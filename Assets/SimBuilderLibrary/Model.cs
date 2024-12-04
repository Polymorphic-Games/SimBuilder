using SimBuilderLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using static UnityEngine.EventSystems.EventTrigger;

public class Model
{
    private ModelLoader ModelLoader;
    private Model InitialModel;
    public string ModelName = "";
    public SSA Driver = new();
    //public DTM dtm_driver = new();

    public Dictionary<int, State> StateDictionary = new();
    public Dictionary<int, Rate> RateDictionary = new();
    public Dictionary<int, Connection> ConnectionDictionary = new();
    public List<List<List<State>>> states = new();

    private event EventHandler<Dictionary<int, State>> UpdateModelStatesEvent;

    private event EventHandler<Dictionary<int, State>> StepForwardModelEvent;

    private event EventHandler<Dictionary<int, State>> StepBackwardModelEvent;

    private event EventHandler<State> AddStateEvent;

    private event EventHandler<State> RemoveStateEvent;

    private event EventHandler<Rate> AddRateEvent;

    private event EventHandler<Rate> RemoveRateEvent;

    private event EventHandler<Connection> AddConnectionEvent;

    private event EventHandler<Connection> RemoveConnectionEvent;

    public Model()
    {
        ModelLoader = new ModelLoader(this);
    }

    public virtual void SetInitialModelState()
    {
    }

    public void ClearModel()
    {
        List<int> uniqueIDs = new();

        foreach (var entry in ConnectionDictionary)
        {
            uniqueIDs.Add(entry.Value.UniqueID);
        }
        foreach (int uniqueId in uniqueIDs)
        {
            RemoveConnection(ConnectionDictionary[uniqueId]);
        }
        uniqueIDs.Clear();

        foreach (var entry in StateDictionary)
        {
            uniqueIDs.Add(entry.Value.UniqueID);
        }
        foreach (int uniqueId in uniqueIDs)
        {
            RemoveState(StateDictionary[uniqueId] as State);
        }
        uniqueIDs.Clear();

        foreach (var entry in RateDictionary)
        {
            uniqueIDs.Add(entry.Value.UniqueID);
        }
        foreach (int uniqueId in uniqueIDs)
        {
            RemoveRate(RateDictionary[uniqueId] as Rate);
        }
        uniqueIDs.Clear();
    }

    public void ResetModel()
    {
        LoadModel(InitialModel);
    }

    public void LoadModel(Model newModel)
    {
        if (newModel == null)
        {
            return;
        }

        ClearModel();
        InitialModel = newModel;
        ModelLoader.LoadModel(newModel);
    }

    public ModelComponent GetDictionaryValue(string name, Dictionary<int, State> dictionary)
    {
        foreach (var entry in dictionary)
        {
            if (entry.Value.Name == name)
            {
                return entry.Value;
            }
        }
        return null;
    }

    public ModelComponent GetDictionaryValue(string name, Dictionary<int, ModelComponent> dictionary)
    {
        foreach (var entry in dictionary)
        {
            if (entry.Value.Name == name)
            {
                return entry.Value;
            }
        }
        return null;
    }

    public State AddState(State state)
    {
        StateDictionary.Add(state.UniqueID, state);
        AddStateEvent?.Invoke(this, state);
        return state;
    }

    public State AddState()
    {
        State state = new();
        state.IsState = true;
        return AddState(state);
    }

    public State AddState(string name, float value)
    {
        State state = new();
        state.Name = name;
        state.InitializePopulation(value);
        return AddState(state);
    }

    public State AddSpatialState(string name, float value, int row, int col)
    {
        State state = AddState(name, value);
        state.row = row;
        state.col = col;
        return state;
    }

    public State RemoveState(State state)
    {
        if (!StateDictionary.ContainsValue(state))
        {
            return state;
        }

        StateDictionary.Remove(state.UniqueID);
        RemoveStateEvent?.Invoke(this, state);
        List<Connection> connectionsToRemove = new();
        foreach (var connection in ConnectionDictionary.Values)
        {
            if (connection.FromState == state || connection.ToState == state)
            {
                connectionsToRemove.Add(connection);
            }
        }

        foreach (var connection in connectionsToRemove)
        {
            RemoveConnection(connection);
        }

        return state;
    }

    public Rate AddRate(Rate rate)
    {
        RateDictionary.Add(rate.UniqueID, rate);
        AddRateEvent?.Invoke(this, rate);
        return rate;
    }

    public Rate AddRate()
    {
        Rate rate = new();
        return AddRate(rate);
    }

    public Rate AddRate(string name, float value, bool isRate = true)
    {
        Rate rate = new();
        rate.Name = name;
        rate.Value = value;
        rate.IsRate = isRate;
        return AddRate(rate);
    }

    public Rate RemoveRate(Rate rate)
    {
        RateDictionary.Remove(rate.UniqueID);
        RemoveRateEvent?.Invoke(this, rate);
        return rate;
    }

    public Connection AddConnection()
    {
        Connection connection = new Connection(this);
        ConnectionDictionary.Add(connection.UniqueID, connection);
        AddConnectionEvent?.Invoke(this, connection);
        return connection;
    }

    public Connection AddConnection(State fromState, State toState)
    {
        if (FindConnection(fromState, toState) == null)
        {
            Connection connection = new Connection(this, fromState, toState);
            ConnectionDictionary.Add(connection.UniqueID, connection);
            AddConnectionEvent?.Invoke(this, connection);
            return connection;
        }
        else
        {
            return null;
        }
    }

    public Connection AddConnectionDuplicates(State fromState, State toState)
    {
        Connection connection = new Connection(this, fromState, toState);
        ConnectionDictionary.Add(connection.UniqueID, connection);
        AddConnectionEvent?.Invoke(this, connection);
        return connection;
    }

    public Connection AddConnection(Connection connection)
    {
        ConnectionDictionary.Add(connection.UniqueID, connection);
        AddConnectionEvent?.Invoke(this, connection);
        foreach (var rateEquationVariable in connection.RateEquation.RateEquationVariables)
        {
            connection.RateEquation.InvokeAddRateEquationVariable(rateEquationVariable);
        }
        return connection;
    }

    public Connection RemoveConnection(Connection connection)
    {
        ConnectionDictionary.Remove(connection.UniqueID);
        foreach (var rateEquationVariable in connection.RateEquation.RateEquationVariables)
        {
            connection.RateEquation.InvokeRemoveRateEquationVariable(rateEquationVariable);
        }
        RemoveConnectionEvent?.Invoke(this, connection);
        return connection;
    }

    public Connection FindConnection(State fromState, State toState)
    {
        foreach (var connection in ConnectionDictionary)
        {
            if (connection.Value.FromState == fromState && connection.Value.ToState == toState)
            {
                return connection.Value;
            }
        }
        return null;
    }

    public void AddUpdateModelStatesEventListener(IUpdateModelStatesHandler obj, bool add = true)
    {
        if (add)
        {
            UpdateModelStatesEvent += obj.UpdateModelStatesCallback;
        }
        else
        {
            UpdateModelStatesEvent -= obj.UpdateModelStatesCallback;
        }
    }

    public void StepBackwardModelEventListener(IStepBackwardModelHandler obj, bool add = true)
    {
        if (add)
        {
            StepBackwardModelEvent += obj.StepBackwardModelCallback;
        }
        else
        {
            StepBackwardModelEvent -= obj.StepBackwardModelCallback;
        }
    }

    public void StepForwardModelEventListener(IStepForwardModelHandler obj, bool add = true)
    {
        if (add)
        {
            StepForwardModelEvent += obj.StepForwardModelCallback;
        }
        else
        {
            StepForwardModelEvent -= obj.StepForwardModelCallback;
        }
    }

    public void AddStateEventListener(IStateHandler obj, bool add = true)
    {
        if (add)
        {
            AddStateEvent += obj.AddStateCallback;
            RemoveStateEvent += obj.RemoveStateCallback;
        }
        else
        {
            AddStateEvent -= obj.AddStateCallback;
            RemoveStateEvent -= obj.RemoveStateCallback;
        }
    }

    public void AddRateEventListener(IRateHandler obj, bool add = true)
    {
        if (add)
        {
            AddRateEvent += obj.AddRateCallback;
            RemoveRateEvent += obj.RemoveRateCallback;
        }
        else
        {
            AddRateEvent -= obj.AddRateCallback;
            RemoveRateEvent -= obj.RemoveRateCallback;
        }
    }

    public void AddConnectionEventListener(IConnectionHandler obj)
    {
        AddConnectionEvent += obj.AddConnectionCallback;
        RemoveConnectionEvent += obj.RemoveConnectionCallback;
    }

    //public void run_DTM(float tau, int iterations)
    //{
    //    for (int i = 0; i < iterations; i++)
    //    {
    //        dtm_driver.run(this, tau);
    //    }
    //}

    public void run_model_tau_leap(float tau, int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            Driver.tau_adaptive(this, tau);
            UpdateModelStatesEvent?.Invoke(this, StateDictionary);
        }
    }

    public void StepForward()
    {
        foreach (var state in StateDictionary)
        {
            if (!state.Value.CacheForward())
            {
                return;
            }
        }
        StepForwardModelEvent?.Invoke(this, StateDictionary);
    }

    public void StepBackward()
    {
        foreach (var state in StateDictionary)
        {
            state.Value.CacheBackward();
        }
        StepBackwardModelEvent?.Invoke(this, StateDictionary);
    }
}