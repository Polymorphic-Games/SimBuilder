using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConnectionHandler
{
    public void AddConnectionCallback(object sender, Connection connection);
    public void RemoveConnectionCallback(object sender, Connection connection);
}
