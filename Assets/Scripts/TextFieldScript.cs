using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextFieldScript : MonoBehaviour
{
    public string GetText()
    {
        return GetComponent<TextMeshProUGUI>().text;
    }
    public void SetText(string text)
    {
        GetComponent<TextMeshProUGUI>().text = text;
    }
}
