using SimBuilderLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RateEquation : IUpdateModelComponentNameHandler
{
    public State FromState;

    public List<State> ModifierStates = new();
    public List<RateEquationVariable> RateEquationVariables = new List<RateEquationVariable>();
    public string RateEquationText;
    public event EventHandler<RateEquationVariable> AddRateEquationVariableEvent;
    event EventHandler<RateEquationVariable> RemoveRateEquationVariableEvent;
    public event EventHandler<string> UpdateRateEquationTextEvent;

    ~RateEquation()
    {
        FromState?.UpdateModelComponentNameEventListener(this, false);
    }
    public RateEquation()
    {
        FromState?.UpdateModelComponentNameEventListener(this);
    }

    public void AddRateEquationVariableEventListener(IRateEquationVariableHandler obj, bool add = true)
    {
        if (add)
        {
            AddRateEquationVariableEvent += obj.AddRateEquationVariableCallback;
            RemoveRateEquationVariableEvent += obj.RemoveRateEquationVariableCallback;
        } else
        {
            AddRateEquationVariableEvent -= obj.AddRateEquationVariableCallback;
            RemoveRateEquationVariableEvent += obj.RemoveRateEquationVariableCallback;
        }
    }

    public void UpdateRateEquationTextEventListener(IUpdateRateEquationTextHandler obj, bool add = true)
    {
        if (add)
        {
            UpdateRateEquationTextEvent += obj.UpdateRateEquationTextCallback;
        } else
        {
            UpdateRateEquationTextEvent -= obj.UpdateRateEquationTextCallback;
        }
    }

    public bool ContainsRateEquationVariable(ModelComponent modelComponent)
    {
        foreach (var rateEquationVariable in RateEquationVariables)
        {
            if (rateEquationVariable.ModelComponent?.UniqueID == modelComponent.UniqueID )
            {
                return true;
            }
        }

        if (modelComponent == FromState)
        {
            return true;
        }

        return false;
    }

    public void UpdateFromState(State newFromState)
    {
        FromState?.UpdateModelComponentNameEventListener(this, false);
        FromState = newFromState;
        FromState?.UpdateModelComponentNameEventListener(this);
        UpdateRateEquationText();
    }

   

    public RateEquationVariable AddRateEquationVariable()
    {
        RateEquationVariable variable = new RateEquationVariable(this);
        RateEquationVariables.Add(variable);
        AddRateEquationVariableEvent?.Invoke(this, variable);
        UpdateRateEquationText();
        return variable;
    }

    public RateEquationVariable AddRateEquationVariable(ModelComponent modelComponent)
    {
        
        if (ContainsRateEquationVariable(modelComponent)) {
            return null;
        }
        
        RateEquationVariable variable = new RateEquationVariable(this, modelComponent); 
        RateEquationVariables.Add(variable);
        AddRateEquationVariableEvent?.Invoke(this, variable);
        UpdateRateEquationText();
        return variable;
    }

    public void InvokeAddRateEquationVariable(RateEquationVariable variable)
    {
        AddRateEquationVariableEvent?.Invoke(this, variable);
    }

    public void InvokeRemoveRateEquationVariable(RateEquationVariable variable)
    {
        RemoveRateEquationVariableEvent?.Invoke(this, variable);
    }

    public RateEquationVariable RemoveRateEquationVariable(RateEquationVariable variable)
    {
        RateEquationVariables.Remove(variable);
        RemoveRateEquationVariableEvent?.Invoke(this, variable);
        UpdateRateEquationText();
        return variable;
    }


    public void UpdateRateEquationText()
    {
        RateEquationText = FromState?.Name;
        foreach (RateEquationVariable rateEquationVariable in RateEquationVariables)
        {
            RateEquationText += rateEquationVariable?.ModelComponent?.Name;
        }
        UpdateRateEquationTextEvent?.Invoke(this, RateEquationText);
    }

    public float CalculateRateEquation()
    {
        if (!RateEquationVariables.Any())
        {
            return 0;
        }

        float dependantVariable = FromState.Value;
        foreach(RateEquationVariable rateEquationVariable in RateEquationVariables)
        {
            if (rateEquationVariable.ModelComponent != null)
            {
                dependantVariable *= rateEquationVariable.ModelComponent.Value;
            }
        }
        return dependantVariable;
    }


    public void UpdateModelComponentNameCallback(object sender, string newName)
    {
        FromState.Name = newName;
        UpdateRateEquationText();
    }




}

