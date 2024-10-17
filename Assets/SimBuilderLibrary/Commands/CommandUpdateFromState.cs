using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateFromState : ICommand
{
    Connection Connection;
    State PrevState;
    State State;

    public CommandUpdateFromState(Connection connection, State state)
    {
        Connection = connection;
        State = state;
    }

    public bool Execute()
    {
        PrevState = Connection.FromState;
        Connection.UpdateFromState(State);
        return true;
    }

    public void Undo()
    {
        Connection.UpdateFromState(PrevState);
    }
}
