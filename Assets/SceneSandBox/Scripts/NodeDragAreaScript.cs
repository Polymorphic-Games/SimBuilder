using SceneSandBox;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeDragAreaScript : DragStateScript
{
    public void InitializeComponent(MediatorComponent mediator)
    {
        base.InitializeComponent(mediator);
        GetComponent<PointerStateScript>().InitializeComponent(this);
    }

    public override void Notify(object sender, params object[] args)
    {
        switch (sender)
        {
            case PointerStateScript:
                mediator.Notify(sender, "NodeDragAreaScript");
                break;
        }
    }
}
