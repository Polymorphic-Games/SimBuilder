using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdatePositionHandler
{
    public void UpdatePositionCallback(object sender, Vector2 position);
}
