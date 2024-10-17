using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdateConnectionFromStateHandler
{
    public void UpdateFromStateCallback(object sender, State state);

}
