using SimBuilderLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rate : ModelComponent
{
    public Rate() : base()
    {
        IsRate = true;
    }

    public Rate(int uniqueID, string name, float value) : this()
    {
        UniqueID = uniqueID;
        Name = name;
        Value = value;
    }

    public object Clone()
    {
        return new Rate(UniqueID, Name, Value);
    }
}


