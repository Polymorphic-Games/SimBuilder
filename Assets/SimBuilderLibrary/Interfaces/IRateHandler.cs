using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRateHandler
{ 
    public void AddRateCallback(object sender, Rate rate);
    public void RemoveRateCallback(object sender, Rate rate);
}
