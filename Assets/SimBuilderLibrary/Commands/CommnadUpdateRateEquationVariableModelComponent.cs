using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommnadUpdateRateEquationVariableModelComponent : ICommand
{
    RateEquationVariable Variable;
    ModelComponent PrevModelComponent;
    ModelComponent ModelComponent;
    
    public CommnadUpdateRateEquationVariableModelComponent(RateEquationVariable variable, ModelComponent modelComponent)
    {
        Variable = variable;
        ModelComponent = modelComponent;
    }

    public bool Execute()
    {
        PrevModelComponent = Variable.ModelComponent;
        Variable.UpdateRateEquationVariableModelComponent(ModelComponent);
        return false;
    }

    public void Undo()
    {
        Variable.UpdateRateEquationVariableModelComponent(PrevModelComponent);
    }

}
