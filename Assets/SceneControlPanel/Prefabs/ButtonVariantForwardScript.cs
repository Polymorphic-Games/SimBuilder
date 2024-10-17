using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonVariantForwardScript : ButtonScript
{
    Model Model;

    public void InitializeComponent(MediatorComponent mediator, Model model)
    {
        base.InitializeComponent(mediator);
        Model = model;
    }

    public override void Notify(object sender, params object[] args)
    {
        Model.StepForward();
    }
}
