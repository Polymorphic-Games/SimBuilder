using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RateEquationFromStateScript : MediatorComponent
{
    public void setValue(string value)
    {
        GetComponent<TextMeshProUGUI>().text = value;
    }

}
