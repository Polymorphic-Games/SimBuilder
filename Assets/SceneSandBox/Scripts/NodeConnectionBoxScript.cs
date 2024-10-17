using SimBuilderLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class NodeConnectionBoxScript : MediatorComponent
{
    Color32 color = new Color32(255, 255, 255, 255);
    Color32 colorPointerOver = new Color32(0, 255, 0, 255);

    public override void InitializeComponent(MediatorComponent mediator)
    {
        base.InitializeComponent(mediator);
        transform.GetComponent<PointerStateScript>().InitializeComponent(this);
        transform.GetComponent<DragStateScript>().InitializeComponent(this);
    }

    public void NotifyPositionHandler()
    {
        GetComponent<PositionHandlerScript>().UpdatePosition(transform.position);
    }

    public override void Notify(object sender, params object[] args)
    {
        switch (sender)
        {
            case PointerStateScript:
                pointerStateHandler((PointerStateScript)sender);
                break;
            case DragStateScript:
                mediator.Notify(this);
                break;
        }
    }

    void pointerStateHandler(PointerStateScript sender)
    {
        switch (sender.pointerState)
        {
            case PointerStateScript.PointerState.OnPointerEnter:
                GetComponent<Image>().color = colorPointerOver;
                break;
            case PointerStateScript.PointerState.OnPointerExit:
                GetComponent<Image>().color = color;
                break;
        }
    }



}

