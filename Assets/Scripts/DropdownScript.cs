using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class DropdownScript : MediatorComponent
{
    public string CaptionText { get; private set; }

    private int PrevValue;
    public int Value { get; private set; }
    public override void InitializeComponent(MediatorComponent mediator)
    {
        base.InitializeComponent(mediator);
    }

    private void Awake()
    {
        GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    void OnValueChanged()
    {
        int newValue = GetComponent<TMP_Dropdown>().value;

        //set to original value and let mediator handle changes
        GetComponent<TMP_Dropdown>().SetValueWithoutNotify(Value);
        Notify(this, newValue);
    }

    public void SetValueWithoutNotify(int value)
    {
        Value = value;
        GetComponent<TMP_Dropdown>().SetValueWithoutNotify(value);
    }


}
