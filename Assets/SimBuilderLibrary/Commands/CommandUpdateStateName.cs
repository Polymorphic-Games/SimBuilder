using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateStateName : ICommand
{
    State State;
    string PrevName;
    string Name;


    public CommandUpdateStateName(State state, string name)
    {
        State = state;
        Name = name;
    }

    public bool Execute()
    {
        PrevName = State.Name;
        State.UpdateName(Name);
        return true;
    }

    public void Undo()
    {
        State.UpdateName(PrevName);
    }
}
