using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionHandlerScript : MonoBehaviour
{
    event EventHandler<Vector2> UpdatePositionHandler;

    public void UpdatePositionEventListener(IUpdatePositionHandler obj, bool add = true)
    {
        if (add)
        {
            UpdatePositionHandler += obj.UpdatePositionCallback;
        }
        else
        {
            UpdatePositionHandler -= obj.UpdatePositionCallback;
        }
    }

    public void UpdatePosition(Vector2 newPosition)
    {
        UpdatePositionHandler?.Invoke(this, newPosition);        
    }

    public void BroadcastCurrentPosition()
    {
        UpdatePositionHandler?.Invoke(this, gameObject.transform.position);
    }
}
