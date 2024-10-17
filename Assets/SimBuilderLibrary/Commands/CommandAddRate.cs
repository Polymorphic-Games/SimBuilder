using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimBuilderLibrary
{
    public class CommandAddRate : ICommand
    {
        Model Model;
        Rate Rate;
        public CommandAddRate(Model model)
        {
            Model = model;
        }
        public bool Execute()
        {
            Rate = Model.AddRate();
            return true;
        }
        public void Undo()
        {
            Model.RemoveRate(Rate);
        }
    }
}
