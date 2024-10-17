using SceneLeftPanel;
using SimBuilderLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RateEquationVarDropdownScript : DropdownScript, IUpdateRateEquationVariableModelComponentHandler, IStateHandler, IRateHandler, IUpdateModelComponentHandler
{
    Model Model;
    RateEquationVariable RateEquationVariable;
    List<State> States = new();
    List<Rate> Rates = new();
    List<string> DropdownOptions = new();
    Dictionary<int, ModelComponent> DropDownValueToModelComponent = new();
    Dictionary<int, int> ModelComponentUniqueIDtoDropDownValue = new();
    int initialDropDownValue = 0;

    void OnDestroy()
    {
        Model.AddStateEventListener(this, false);
        Model.AddRateEventListener(this, false);
        RateEquationVariable.UpdateRateEquationVariableModelComponentEventListener(this, false);
        foreach (var state in Model.StateDictionary)
        {
            state.Value.UpdateModelComponentEventListener(this,false);
        }
        foreach (var rate in Model.RateDictionary)
        {
            rate.Value.UpdateModelComponentEventListener(this, false);
        }
    }

    public void InitializeComponent(MediatorComponent mediator, Model model, RateEquationVariable rateEquationVariable)
    {
        base.InitializeComponent(mediator);
        Model = model;
        
        string modelComponentName = GetComponent<TMP_Dropdown>().captionText.text;
        RateEquationVariable = rateEquationVariable;

        Model.AddStateEventListener(this);
        Model.AddRateEventListener(this);
        RateEquationVariable.UpdateRateEquationVariableModelComponentEventListener(this);
        foreach (var state in Model.StateDictionary)
        {
            States.Add(state.Value as State);
            state.Value.UpdateModelComponentEventListener(this);
        }
        foreach (var rate in Model.RateDictionary)
        {
            Rates.Add(rate.Value as Rate);
            rate.Value.UpdateModelComponentEventListener(this);
        }
        RebuildDropDownOptions();

        if (rateEquationVariable.ModelComponent != null)
        {
            initialDropDownValue = ModelComponentUniqueIDtoDropDownValue[rateEquationVariable.ModelComponent.UniqueID];
        }
        SetValueWithoutNotify(initialDropDownValue);
    }

    public override void Notify(object sender, params object[] args)
    {
        switch (sender)
        {
            case DropdownScript:
                OnNotificationDropDownScript(sender as DropdownScript, args);
                break;
            default:
                base.Notify(sender, args);
                break;
        }
    }

    void OnNotificationDropDownScript(DropdownScript sender, params object[] args)
    {
        int dropdownValue = (int)args[0];
        ModelComponent modelComponent;

        if (dropdownValue == 0)
        {
            modelComponent = null;
        }
        else
        {
            modelComponent = DropDownValueToModelComponent[dropdownValue];
        }

        commandStack.Push(new CommnadUpdateRateEquationVariableModelComponent(RateEquationVariable, modelComponent));
        if (!commandStack.Peek().Execute())
        {
            commandStack.Pop();
        }
    }

    void RebuildDropDownOptions()
    {
        int DropDownValue = 1;

        DropdownOptions.Clear();
        DropDownValueToModelComponent.Clear();
        ModelComponentUniqueIDtoDropDownValue.Clear();
        DropdownOptions.Add("");
        foreach (State state in States)
        {
            DropdownOptions.Add(state.Name);
            ModelComponentUniqueIDtoDropDownValue.Add(state.UniqueID, DropDownValue);
            DropDownValueToModelComponent.Add(DropDownValue++, state);
        }
        foreach (Rate rate in Rates)
        {
            DropdownOptions.Add(rate.Name);
            ModelComponentUniqueIDtoDropDownValue.Add(rate.UniqueID, DropDownValue);
            DropDownValueToModelComponent.Add(DropDownValue++, rate);
        }

        GetComponent<TMP_Dropdown>().ClearOptions();
        GetComponent<TMP_Dropdown>().AddOptions(DropdownOptions);
    }

    public void UpdateRateEquationVariableModelComponentCallback(object sender, ModelComponent newModelComponent)
    {
        if (newModelComponent == null)
        {
            GetComponent<TMP_Dropdown>().SetValueWithoutNotify(0);
            return;
        }

        RateEquationVariable = (RateEquationVariable)sender;
        int indexValue = ModelComponentUniqueIDtoDropDownValue[newModelComponent.UniqueID];
        SetValueWithoutNotify(indexValue);
    }

    public void AddStateCallback(object sender, State newState)
    {
        //if dropdown current value is a rate, increase the value to account for state option insert
        int newValue=Value;
        if (Value >= States.Count + 1)
        {
            newValue++;
        }

        States.Add(newState);
        newState.UpdateModelComponentEventListener(this);


        RebuildDropDownOptions();
        SetValueWithoutNotify(newValue);
    }

    public void RemoveStateCallback(object sender, State state)
    {
        States.Remove(state);
        state.UpdateModelComponentEventListener(this, false);
        RebuildDropDownOptions();
    }

    public void AddRateCallback(object sender, Rate newRate)
    {
        Rates.Add(newRate);
        newRate.UpdateModelComponentEventListener(this);
        RebuildDropDownOptions();
    }

    public void RemoveRateCallback(object sender, Rate rate)
    {
        Rates.Remove(rate);
        rate.UpdateModelComponentEventListener(this, false);
        RebuildDropDownOptions();
    }

    public void UpdateModelComponentCallback(object sender, ModelComponent updatedModelComponent)
    {
        RebuildDropDownOptions();
        SetValueWithoutNotify(Value);
    }
}
