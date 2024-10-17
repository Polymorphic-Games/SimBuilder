using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceUpdateCanvas : MonoBehaviour {

    //Canvas canvas;
    public RectTransform layoutPanel;

    void Start() {

        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutPanel);

        //canvas = GetComponent<Canvas>();
        //canvas.ForceUpdateCanvases();
    }

    private void Update()
    {
        ForceUpdate();

    }
    public void ForceUpdate() {
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutPanel);
    }
}