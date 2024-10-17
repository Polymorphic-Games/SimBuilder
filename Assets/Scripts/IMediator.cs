using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IMediator
{
    void Notify(object sender, params object[] args);

}

