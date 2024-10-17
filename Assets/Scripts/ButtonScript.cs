using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MediatorComponent
{
    public int State { get; private set; }


    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    public void OnClick()
    {
        Notify(this);
    }

    public void SetInteractable(bool interactable)
    {
        float alpha = 0.75f;
        GetComponent<Button>().interactable = interactable;
        foreach(Image sprite in GetComponentsInChildren<Image>())
        {
            if (interactable)
            {
                sprite.color += new Color(0, 0, 0, alpha);
            }
            else
            {
                sprite.color -= new Color(0, 0, 0, alpha);
            }
        }
    }
}
