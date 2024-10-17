using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonResetScript : ButtonScript
{
    Model Model;

    public void InitializeComponent(MediatorComponent mediator, Model model)
    {
        base.InitializeComponent(mediator);
        Model = model;
    }
}
