using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateContainerColorScript : MonoBehaviour
{
    public void setColor(Color color)
    {
        GetComponent<Image>().color = color;
    }
}
