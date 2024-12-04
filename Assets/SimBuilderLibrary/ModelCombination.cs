using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ModelCombination : Model
{
    public Model spatialModel = new();
    private Model populationModel = new();
    private Connection spatialConnection;

    private Dictionary<int, int> stateUniqueIDtoSpatialUniqueID = new();
    private Dictionary<int, int> rateUniqueIDtoSpatialUniqueID = new();

    public ModelCombination()
    {
        //createPopulationSpatialModel(populationModel, gridSize);
    }

    public Model createPopulationSpatialModel(Model populationModel, int gridSize)
    {
        int populationModelUniqueID;
        int spatialModelUniqueID;

        this.populationModel = populationModel;
        spatialModel = new Model();

        var watch = new Stopwatch();

        foreach (Rate rate in populationModel.RateDictionary.Values)
        {
            Rate spatialRate = spatialModel.AddRate(rate);
            rateUniqueIDtoSpatialUniqueID.Add(rate.UniqueID, spatialRate.UniqueID);
        }

        for (int i = 0; i < gridSize; i++)
        {
            states.Add(new List<List<State>>());
            for (int j = 0; j < gridSize; j++)
            {
                stateUniqueIDtoSpatialUniqueID = new();
                states[i].Add(new List<State>());
                foreach (State state in populationModel.StateDictionary.Values)
                {
                    State spatialState = spatialModel.AddSpatialState(state.Name, state.Value, i, j);
                    stateUniqueIDtoSpatialUniqueID.Add(state.UniqueID, spatialState.UniqueID);
                    states[i][j].Add(spatialState);
                }

                foreach (Connection connection in populationModel.ConnectionDictionary.Values)
                {
                    spatialConnection = spatialModel.AddConnection();

                    populationModelUniqueID = connection.FromState.UniqueID;
                    spatialModelUniqueID = stateUniqueIDtoSpatialUniqueID[populationModelUniqueID];
                    spatialConnection.FromState = spatialModel.StateDictionary[spatialModelUniqueID];

                    populationModelUniqueID = connection.ToState.UniqueID;
                    spatialModelUniqueID = stateUniqueIDtoSpatialUniqueID[populationModelUniqueID];
                    spatialConnection.ToState = spatialModel.StateDictionary[spatialModelUniqueID];

                    foreach (RateEquationVariable variable in connection.RateEquation.RateEquationVariables)
                    {
                        populationModelUniqueID = variable.ModelComponent.UniqueID;
                        if (variable.ModelComponent.IsState)
                        {
                            spatialModelUniqueID = stateUniqueIDtoSpatialUniqueID[populationModelUniqueID];
                            spatialConnection.RateEquation.AddRateEquationVariable(spatialModel.StateDictionary[spatialModelUniqueID]);
                        }
                        else if (variable.ModelComponent.IsRate)
                        {
                            spatialModelUniqueID = rateUniqueIDtoSpatialUniqueID[populationModelUniqueID];
                            spatialConnection.RateEquation.AddRateEquationVariable(spatialModel.RateDictionary[spatialModelUniqueID]);
                        }
                    }
                }
            }
        }

        //spatial connections
        Rate transportRate = spatialModel.AddRate("t", 0.1f);

        watch.Start();
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                for (int k = 0; k < states[i][j].Count; k++)
                {
                    if (j + 1 < gridSize)
                    {
                        spatialConnection = spatialModel.AddConnectionDuplicates(states[i][j][k], states[i][j + 1][k]);
                        spatialConnection.RateEquation.AddRateEquationVariable(transportRate);
                    }
                    if (j - 1 >= 0)
                    {
                        spatialConnection = spatialModel.AddConnectionDuplicates(states[i][j][k], states[i][j - 1][k]);
                        spatialConnection.RateEquation.AddRateEquationVariable(transportRate);
                    }
                    if (i + 1 < gridSize)
                    {
                        spatialConnection = spatialModel.AddConnectionDuplicates(states[i][j][k], states[i + 1][j][k]);
                        spatialConnection.RateEquation.AddRateEquationVariable(transportRate);
                    }
                    if (i - 1 >= 0)
                    {
                        spatialConnection = spatialModel.AddConnectionDuplicates(states[i][j][k], states[i - 1][j][k]);
                        spatialConnection.RateEquation.AddRateEquationVariable(transportRate);
                    }
                }
            }
        }
        watch.Stop();
        UnityEngine.Debug.Log("Spatial Connections: " + watch.ElapsedMilliseconds);

        spatialModel.states = states;
        return spatialModel;
    }
}