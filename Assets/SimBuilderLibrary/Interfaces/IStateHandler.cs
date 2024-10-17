using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateHandler
{
    public void AddStateCallback(object sender, State state);
    public void RemoveStateCallback(object sender, State state);
}
