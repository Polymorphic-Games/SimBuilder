using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdateConnectionToStateHandler
{
    public void UpdateToStateCallback(object sender, State state);

}
