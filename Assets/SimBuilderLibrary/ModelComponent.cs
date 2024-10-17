using SimBuilderLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelComponent
{
    private string name;
    private float value;
    private float newValue;
    private int uniqueID;
    private static int NextUniqueID;

    public bool IsState = false;
    public bool IsRate = false;

    public ModelComponent()
    {
        UniqueID = NextUniqueID;
        NextUniqueID++;
    }

    event EventHandler<ModelComponent> UpdateModelComponentHandler;
    event EventHandler<string> UpdateModelComponentNameHandler;
    event EventHandler<float> UpdateModelComponentValueHandler;

    public string Name { get; set;}
    public float Value { get; set; }
    public float NewValue { get; set; }
    public int UniqueID { get; protected set; }


    public void UpdateModelComponentEventListener(IUpdateModelComponentHandler obj, bool add = true)
    {
        if (add)
        {
            UpdateModelComponentHandler += obj.UpdateModelComponentCallback;
        }
        else
        {
            UpdateModelComponentHandler -= obj.UpdateModelComponentCallback;
        }
    }
    public void UpdateModelComponentNameEventListener(IUpdateModelComponentNameHandler obj, bool add = true)
    {
        if (add)
        {
            UpdateModelComponentNameHandler += obj.UpdateModelComponentNameCallback;
        } else
        {
            UpdateModelComponentNameHandler -= obj.UpdateModelComponentNameCallback;
        }
    }

    public void UpdateModelComponentValueEventListener(IUpdateModelComponentValueHandler obj, bool add = true)
    {
        if (add)
        {
            UpdateModelComponentValueHandler += obj.UpdateModelComponentValueCallback;
        } else
        {
            UpdateModelComponentValueHandler -= obj.UpdateModelComponentValueCallback;
        }
    }

    public virtual void UpdateName(string name)
    {
        UpdateModelComponentNameHandler?.Invoke(this, name);
        Name = name;
        UpdateModelComponentHandler?.Invoke(this, this);
    }

    public void UpdateValue(float value)
    {
        UpdateModelComponentValueHandler?.Invoke(this, value);
        Value = value;
        UpdateModelComponentHandler?.Invoke(this, this);
    }
}
