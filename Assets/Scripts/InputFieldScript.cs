using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldScript : MediatorComponent
{
    public string Text {get; private set;}

    private void Awake()
    {
        GetComponent<TMP_InputField>().onEndEdit.AddListener(delegate { OnEndEdit(); });
    }

    void OnEndEdit()
    {
        string newText = GetComponent<TMP_InputField>().text;

        //set back to original value and let mediator handle changes
        GetComponent<TMP_InputField>().SetTextWithoutNotify(Text);
        mediator.Notify(this, newText);
    }

    public void SetTextWithoutNotify(string text)
    {
        Text = text;
        GetComponent<TMP_InputField>().SetTextWithoutNotify(text);
    }

    public void resetText()
    {
        GetComponent<TMP_InputField>().text = Text;
    }

}
