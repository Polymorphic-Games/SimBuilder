using SimBuilderLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class Connection : ICloneable
{
    public int UniqueID { get; private set; }
    static int NextUniqueID;
    public Model Model { get; set; }
    public State FromState;
    public State ToState;
    public RateEquation RateEquation = new();

    event EventHandler<State> UpdateFromStateHandler;
    event EventHandler<State> UpdateToStateHandler;

    public Connection(Model model)
    {
        Model = model;
        UniqueID = NextUniqueID;
        NextUniqueID++;
    }

    public Connection(Model model, State fromState, State toState) : this(model)
    {
        UpdateFromState(fromState);
        UpdateToState(toState);
    }

    public Connection(int uniqueID)
    {
        UniqueID = uniqueID;
    }

    public object Clone()
    {
        return new Connection(UniqueID);
    }

    public bool ContainsConnection()
    {
        foreach (var connection in Model.ConnectionDictionary.Values) {

            if (connection != this && connection.FromState == FromState && connection.ToState == ToState)
            {
                return true;
            }
        }
        return false;
    }

    public void UpdateFromStateEventListener(IUpdateConnectionFromStateHandler obj, bool add = true)
    {
        if (add)
        {
            UpdateFromStateHandler += obj.UpdateFromStateCallback;
        }
        else
        {
            UpdateFromStateHandler -= obj.UpdateFromStateCallback;
        }
    }

    public void UpdateToStateEventListener(IUpdateConnectionToStateHandler obj, bool add = true)
    {
        if (add)
        {
            UpdateToStateHandler += obj.UpdateToStateCallback;
        }
        else
        {
            UpdateToStateHandler -= obj.UpdateToStateCallback;
        }
    }
    
    public void UpdateFromState(State newFromStateName)
    {
        State prevState = FromState;
        FromState = newFromStateName;

        if (ContainsConnection())
        {
            FromState = prevState;
            UpdateFromStateHandler?.Invoke(this, null);
            return;
        }

        UpdateFromStateHandler?.Invoke(this, newFromStateName);
        RateEquation.UpdateFromState(newFromStateName);
    }

    public void UpdateToState(State newToState)
    {
        State prevState = newToState;
        ToState = newToState;

        if (ContainsConnection())
        {
            ToState = prevState;
            UpdateToStateHandler?.Invoke(this, null);
            return;
        }

        UpdateToStateHandler?.Invoke(this, newToState);
    }


}
