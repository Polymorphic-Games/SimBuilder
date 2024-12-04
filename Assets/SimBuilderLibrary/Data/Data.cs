using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class Data : MonoBehaviour
{
    public int numPopulationModelBins;
    private int numRateEquationStateVariablesMax = 5;

    private Dictionary<int, int> stateUniqueIDtoPopulationIndex = new();
    public Dictionary<string, PopulationGroup> nameToPopStruct = new();
    public int[] population;
    public ConnectionComponent[] connections;
    public int[] rateEquationStateVariableIndices;

    [BurstCompile]
    public struct ConnectionComponent
    {
        public int From;
        public int To;
        public float Rate;
        public int deltaP;
    }

    public class PopulationGroup
    {
        public List<int> populationIndices = new();
        public List<int> population = new();
        public List<int> row = new();
        public List<int> col = new();
    }

    public void modelToNativeData(Model model)
    {
        int i = 0;
        float rateProduct = 1;
        int stateCounter = 0;

        population = new int[model.StateDictionary.Count];
        rateEquationStateVariableIndices = new int[model.ConnectionDictionary.Count * 10];
        connections = new ConnectionComponent[model.ConnectionDictionary.Count];
        nameToPopStruct.Add("I", new PopulationGroup());



        //foreach (State state in model.StateDictionary.Values)
        //{
        //    population[i] = (int)state.Value;
        //    stateUniqueIDtoPopulationIndex.Add(state.UniqueID, i);
        //    i++;
        //}

        foreach (var listListStates in model.states)
        {
            foreach (var listStates in listListStates)
            {
                foreach (var state in listStates)
                {
                    population[i] = (int)state.Value;
                    stateUniqueIDtoPopulationIndex.Add(state.UniqueID, i);
                

                    if (state.Name == "I")
                    {
                        nameToPopStruct["I"].populationIndices.Add(i);
                        nameToPopStruct["I"].population.Add(0);
                        nameToPopStruct["I"].row.Add(state.row);
                        nameToPopStruct["I"].col.Add(state.col);
                    }
                    //Debug.Log(state.Name + " " + state.row + " " + state.col);
                    i++;
                }
            }
        }

        i = 0;
        foreach (Connection connection in model.ConnectionDictionary.Values)
        {
            rateProduct = 1;
            stateCounter = 0;

            foreach (RateEquationVariable variable in connection.RateEquation.RateEquationVariables)
            {
                if (variable.ModelComponent.IsRate)
                {
                    rateProduct *= variable.ModelComponent.Value;
                }
                else if (variable.ModelComponent.IsState && stateCounter < numRateEquationStateVariablesMax)
                {
                    rateEquationStateVariableIndices[i * 5 + stateCounter] = stateUniqueIDtoPopulationIndex[variable.ModelComponent.UniqueID];
                    stateCounter++;
                }
            }

            //fill remaining rateEquationStateVariableIndices with -1
            for (int j = stateCounter; j < 5; j++)
            {
                rateEquationStateVariableIndices[i * 5 + j] = -1;
            }

            //new  ConnectionComponent
            connections[i] = new ConnectionComponent
            {
                From = stateUniqueIDtoPopulationIndex[connection.FromState.UniqueID],
                To = stateUniqueIDtoPopulationIndex[connection.ToState.UniqueID],
                Rate = rateProduct
            };

            i++;
        }
    }

    public void nativeDataToModel()
    {
    }
}