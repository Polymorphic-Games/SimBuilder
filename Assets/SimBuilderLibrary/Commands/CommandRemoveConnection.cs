using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRemoveConnection : ICommand
{
    Model Model;
    Connection Connection;
    public CommandRemoveConnection(Model model, Connection connection)
    {
        Model = model;
        Connection = connection;
    }
    public bool Execute()
    {
        Connection = Model.RemoveConnection(Connection);
        return true;
    }
    public void Undo()
    {
        Model.AddConnection(Connection);
    }
}
