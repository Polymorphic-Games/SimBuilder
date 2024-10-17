using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonVariantBackwardScript : ButtonScript
{
    Model Model;

    public void InitializeComponent(MediatorComponent mediator, Model model)
    {
        base.InitializeComponent(mediator);
        Model = model;
    }

    public override void Notify(object sender, params object[] args)
    {
        Model.StepBackward();
    }
}
