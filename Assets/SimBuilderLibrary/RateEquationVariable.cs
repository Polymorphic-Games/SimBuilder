using SimBuilderLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateEquationVariable : IUpdateModelComponentNameHandler
{

    private int uniqueID;
    private static int NextUniqueID;
    public ModelComponent ModelComponent { get; set; }
    public RateEquation RateEquation { get; set; }

    event EventHandler<ModelComponent> UpdateRateEquationVariableModelComponentEvent;
    ~RateEquationVariable()
    {
        ModelComponent?.UpdateModelComponentNameEventListener(this, false);
    }

    public RateEquationVariable(RateEquation rateEquation)
    {
        UniqueID = NextUniqueID;
        NextUniqueID++;
        RateEquation = rateEquation;
        ModelComponent?.UpdateModelComponentNameEventListener(this);
    }

    public RateEquationVariable(RateEquation rateEquation, ModelComponent modelComponent) : this(rateEquation)
    {
        ModelComponent = modelComponent;
    }

    public void UpdateRateEquationVariableModelComponentEventListener(IUpdateRateEquationVariableModelComponentHandler obj, bool add = true)
    {
        if (add)
        {
            UpdateRateEquationVariableModelComponentEvent += obj.UpdateRateEquationVariableModelComponentCallback;
        } else
        {
            UpdateRateEquationVariableModelComponentEvent -= obj.UpdateRateEquationVariableModelComponentCallback;
        }
    }

    public int UniqueID { get; }
    
    public void UpdateModelComponentNameCallback(object sender, string newName)
    {
        ModelComponent.Name = newName;
        RateEquation.UpdateRateEquationText();
        
    }

    public ModelComponent UpdateRateEquationVariableModelComponent(ModelComponent newModelComponent)
    {
        if (newModelComponent == null)
        {
            ModelComponent?.UpdateModelComponentNameEventListener(this, false);
            UpdateRateEquationVariableModelComponentEvent?.Invoke(this, newModelComponent);
            ModelComponent = newModelComponent;
            RateEquation.UpdateRateEquationText();
            return null;
        }
        else if (RateEquation.ContainsRateEquationVariable(newModelComponent))
        {
            return null;
        }
        else if (newModelComponent == ModelComponent)
        {
            return null;
        }
        else
        {
            ModelComponent?.UpdateModelComponentNameEventListener(this, false);
            newModelComponent?.UpdateModelComponentNameEventListener(this);
            UpdateRateEquationVariableModelComponentEvent?.Invoke(this, newModelComponent);
            ModelComponent = newModelComponent;
            RateEquation.UpdateRateEquationText();
            return newModelComponent;
        }

    }

}
