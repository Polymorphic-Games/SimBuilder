using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRemoveRate : ICommand
{
    Model Model;
    Rate Rate;
    public CommandRemoveRate(Model model, Rate rate)
    {
        Model = model;
        Rate = rate;
    }
    public bool Execute()
    {
        Rate = Model.RemoveRate(Rate);
        return true;
    }
    public void Undo()
    {
        Model.AddRate(Rate);
    }
}
