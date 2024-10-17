using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateModelComponentName : ICommand
{
    ModelComponent ModelComponent;
    string PrevName;
    string Name;

    public CommandUpdateModelComponentName(ModelComponent modelComponent, string name)
    {
        ModelComponent = modelComponent;
        Name = name;
    }

    public bool Execute()
    {
        PrevName = ModelComponent.Name;
        ModelComponent.UpdateName(Name);
        return true;
    }

    public void Undo()
    {
        ModelComponent.UpdateName(Name);
    }
}
