using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerStateScript : MediatorComponent, IPointerEnterHandler, IPointerExitHandler
{
    public PointerEventData eventData;
    public PointerState pointerState;

    public enum PointerState
    {
        OnPointerEnter,
        OnPointerExit,
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.eventData = eventData;
        pointerState = PointerState.OnPointerEnter;
        if (mediator != null) { mediator.Notify(this); }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.eventData = eventData;
        pointerState = PointerState.OnPointerExit;
        if (mediator != null) { mediator.Notify(this); }
    }
    
}
