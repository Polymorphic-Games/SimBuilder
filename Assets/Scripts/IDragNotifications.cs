using SceneSandBox;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IDragNotifications
{
    //public override void Notify(object sender, params object[] args)
    //{
    //    switch (sender)
    //    {
    //        case DragStateScript:
    //            DragStateScript dragStateSender = (DragStateScript)sender;
    //            switch (dragStateSender.dragState)
    //            {
    //                case DragStateScript.DragState.OnDrop:
    //                    if (dragStateSender.eventData.pointerDrag.GetComponent<NodeUpAndOutScript>() != null)
    //                    {
    //                        State state = dragStateSender.eventData.pointerDrag.GetComponentInParent<NodeScript>().State;
    //                        RateEquation.AddRateEquationVariable(state);
    //                        //mediator.Notify(this, InstanceRateEquationVariable);
    //                    }
    //                    break;
    //            }
    //            break;
    //        default:
    //            mediator.Notify(sender, args);
    //            break;
    //    }
    //}
}
