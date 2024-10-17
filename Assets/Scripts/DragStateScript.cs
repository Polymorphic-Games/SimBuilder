using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragStateScript : MediatorComponent, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public PointerEventData eventData;
    public DragState dragState;
    public enum DragState
    { 
        OnBeginDrag,
        OnDrag,
        EndDrag,
        OnDrop
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.eventData = eventData;
        dragState = DragState.OnBeginDrag;
        if (mediator != null) { mediator.Notify(this); }
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.eventData = eventData;
        dragState = DragState.OnDrag;
        if (mediator != null) { mediator.Notify(this, dragState); }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.eventData = eventData;
        dragState = DragState.EndDrag;
        if (mediator != null) { mediator.Notify(this); }
    }

    public void OnDrop(PointerEventData eventData)
    {
        this.eventData = eventData;
        dragState = DragState.OnDrop;
        if (mediator != null) { mediator.Notify(this); }
    }
}
