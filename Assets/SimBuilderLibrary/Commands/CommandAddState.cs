using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimBuilderLibrary
{
    public class CommandAddState : ICommand
    {
        Model Model;
        State State;
        public CommandAddState(Model model)
        {
            Model = model;
        }
        public bool Execute() 
        {
            State = Model.AddState();
            return true;
        }
        public void Undo() 
        {
            Model.RemoveState(State);
        }
    }
}
