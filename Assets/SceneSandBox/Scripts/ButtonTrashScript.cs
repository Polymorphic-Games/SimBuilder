using SceneSandBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrashScript : MediatorComponent
{
    public override void InitializeComponent(MediatorComponent mediator)
    {
        base.InitializeComponent(mediator);
        GetComponent<DragStateScript>().InitializeComponent(this);
    }

    public override void Notify(object sender, params object[] args)
    {
        switch (sender)
        {
            case DragStateScript:
                DragStateScript dragStateSender = sender as DragStateScript;
                switch (dragStateSender.dragState)
                {
                    case DragStateScript.DragState.OnDrop:
                        if (dragStateSender.eventData.pointerDrag != null)
                        {
                            destroyGameObject(dragStateSender.eventData.pointerDrag.gameObject);
                        }
                        break;
                }
                break;
        }
    }

    void destroyGameObject(GameObject gameObject)
    {
        object component = gameObject.GetComponent<NodeDragAreaScript>();

        switch (component)
        {
            case NodeDragAreaScript:

                GameObject nodeGameObject = ((NodeDragAreaScript) component).GetComponentInParent<NodeScript>().gameObject;
                Destroy(nodeGameObject);
                break;
        }
    }
}

