using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRemoveState : ICommand
{
    Model Model;
    State State;
    public CommandRemoveState(Model model, State state)
    {
        Model = model;
        State = state;
    }
    public bool Execute()
    {
        State = Model.RemoveState(State);
        return State != null ? true : false;
    }
    public void Undo()
    {
        Model.AddState(State);
    }
}
