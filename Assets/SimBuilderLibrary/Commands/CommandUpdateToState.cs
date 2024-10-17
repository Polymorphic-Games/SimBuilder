using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandUpdateToState : ICommand
{
    Connection Connection;
    State PrevState;
    State State;

    public CommandUpdateToState(Connection connection, State state)
    {
        Connection = connection;
        State = state;
    }

    public bool Execute() 
    {
        PrevState = Connection.ToState;
        Connection.UpdateToState(State);
        return true;
    }

    public void Undo()
    {
        Connection.UpdateToState(PrevState);
    }
}
