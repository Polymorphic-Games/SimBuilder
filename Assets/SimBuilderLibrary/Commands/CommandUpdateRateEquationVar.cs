using SceneLeftPanel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateRateEquationVar : ICommand
{
    RateEquationVariable RateEquationVariable;
    ModelComponent PrevModelComponent;
    ModelComponent ModelComponent;

    public CommandUpdateRateEquationVar(RateEquationVariable rateEquationVariable, ModelComponent modelComponent)
    {
        RateEquationVariable = rateEquationVariable;
        ModelComponent = modelComponent;
    }

    public bool Execute()
    {
        PrevModelComponent = RateEquationVariable.ModelComponent;
        RateEquationVariable.UpdateRateEquationVariableModelComponent(ModelComponent);
        return true;
    }

    public void Undo()
    {
        RateEquationVariable.UpdateRateEquationVariableModelComponent(PrevModelComponent);
    }

}
