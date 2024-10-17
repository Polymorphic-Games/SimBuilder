using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ModelLoader
{
    Model Model;
    Model LoadedModel;
    Dictionary<int, State> IDtoStateClone = new();
    Dictionary<int, Rate> IDtoRateClone = new();

    public ModelLoader(Model model)
    {
        Model = model;
    }

    public void LoadModel(Model newModel)
    { 
        foreach (var entry in newModel.StateDictionary)
        {
            State stateClone = entry.Value.Clone() as State;
            Model.AddState(stateClone as State);
            IDtoStateClone.Add(stateClone.UniqueID, stateClone);
        }

        foreach (var entry in newModel.RateDictionary)
        {
            Rate rateClone = entry.Value.Clone() as Rate;
            Model.AddRate(rateClone);
            IDtoRateClone.Add(rateClone.UniqueID, rateClone);
        }

        foreach(var entry in newModel.ConnectionDictionary)
        {
            Connection connectionOriginal = entry.Value;
            Connection connectionClone = connectionOriginal.Clone() as Connection;
            connectionClone.Model = newModel;
            Model.AddConnection(connectionClone);

            int fromStateID = connectionOriginal.FromState.UniqueID;
            State fromStateClone = IDtoStateClone[fromStateID];
            connectionClone.UpdateFromState(fromStateClone);

            int toStateID = connectionOriginal.ToState.UniqueID;
            State toStateClone = IDtoStateClone[toStateID];
            connectionClone.UpdateToState(toStateClone);

            foreach(var rateEquationVariable in connectionOriginal.RateEquation.RateEquationVariables)
            {
                int modelComponentID = rateEquationVariable.ModelComponent.UniqueID;

                if (rateEquationVariable.ModelComponent.IsState)
                {
                    State stateClone = IDtoStateClone[modelComponentID];
                    connectionClone.RateEquation.AddRateEquationVariable(stateClone);
                } else if (rateEquationVariable.ModelComponent.IsRate)
                {
                    Rate rateClone = IDtoRateClone[modelComponentID];
                    connectionClone.RateEquation.AddRateEquationVariable(rateClone);
                }
            }             
        }

        IDtoStateClone.Clear();
        IDtoRateClone.Clear();

    }
}
