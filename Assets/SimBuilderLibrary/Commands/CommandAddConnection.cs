using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimBuilderLibrary
{
    public class CommandAddConnection : ICommand
    {
        Model Model;
        Connection Connection;
        public CommandAddConnection(Model model)
        {
            Model = model;
        }
        public bool Execute()
        {
            Connection = Model.AddConnection();
            return true;
        }
        public void Undo()
        {
            Model.RemoveConnection(Connection);
        }
    }
}

