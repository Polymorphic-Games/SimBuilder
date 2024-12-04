using SimBuilderLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


public class State : ModelComponent, ICloneable
{
    public Color32 color { get; set; }
    public int row { get; set; }
    public int col { get; set; }

    private const int CacheLimit = 1000;
    public LinkedList<float> PopulationCache { get; private set; } = new();
    public LinkedListNode<float> CachePosition { get; private set; }
    public State() : base()
    {
        IsState = true;
    }

    public State(int uniqueID, string name, float value) : this()
    {
        UniqueID = uniqueID;
        Name = name;
        InitializePopulation(value);
    }

    public object Clone()
    {
        return new State(UniqueID, Name, Value);
    }

    public void InitializePopulation(float value)
    {
        UpdateValue(value);
        PopulationCache.AddLast(value);
        CachePosition = PopulationCache.First;
    }

    public void UpdatePopulation(float value)
    {
        UpdateValue(value);

        if (PopulationCache.Count < CacheLimit)
        {
            PopulationCache.AddLast(value);
            CachePosition = CachePosition?.Next;
        } 
        else
        {
            CachePosition = CachePosition?.Next;
            //if at end of list
            if (CachePosition == null)
            {
                PopulationCache.RemoveFirst();
                PopulationCache.AddLast(value);
                CachePosition = PopulationCache.Last;
            } else
            {
                CachePosition.Value = value;
            }
        }        
    }

    public bool CacheForward()
    {
        if (CachePosition.Next != null)
        {
            CachePosition = CachePosition.Next;
            UpdateValue(CachePosition.Value);
            return true;
        }
        return false;
    }

    public bool CacheBackward()
    {
        if (CachePosition.Previous != null)
        {
            CachePosition = CachePosition?.Previous;
            UpdateValue(CachePosition.Value);
            return true;
        }
        return false;
    }
}
