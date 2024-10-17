using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeColorScript : MonoBehaviour
{
    public void setColor(Color32 color)
    {
        GetComponent<Image>().color = color;
    }
}
