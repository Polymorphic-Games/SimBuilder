using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateModelComponentValue : ICommand
{
    ModelComponent ModelComponent;
    float PrevValue;
    float Value;

    public CommandUpdateModelComponentValue(ModelComponent modelComponent, float value)
    {
        ModelComponent = modelComponent;
        Value = value;
    }

    public bool Execute()
    {
        PrevValue = ModelComponent.Value;
        ModelComponent.UpdateValue(Value);
        return true;
    }

    public void Undo()
    {
        ModelComponent.UpdateValue(PrevValue);
    }
}
