using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

namespace SceneLeftPanel
{
    public class ConnectionsLayoutContainerAddButtonScript : ButtonScript
    {
        public void setColor(Color32 color)
        {
            GetComponent<Image>().color = color;
        }
    }
}

