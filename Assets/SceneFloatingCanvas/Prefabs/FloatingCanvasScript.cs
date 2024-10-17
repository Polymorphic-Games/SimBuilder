using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FloatingCanvasScript : MediatorComponent
{
    RectTransform panel;
    public Vector2 heightRange = new Vector2(55f, 500f);
    bool ExpandToggleState = true;
    float activeSmooth = 0f;
    GameObject ContentContainer;

    public override void InitializeComponent(MediatorComponent mediator)
    {        
        base.InitializeComponent(mediator);
        GetComponentInChildren<FloatingCanvasExpandToggleButtonScript>().InitializeComponent(this);       
    }
    void Start()
    {
        panel = GetComponent<RectTransform>();
        ContentContainer = GetComponentInChildren<FloatingCanvasContentContainerScript>().gameObject;
        panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ExpandToggleState ? heightRange.y : heightRange.x);
    }

    public override void Notify(object sender, params object[] args)
    {
        switch (sender)
        {
            case FloatingCanvasExpandToggleButtonScript:
                FloatingCanvasExpandToggleButtonScript expandToggleButtonScript = sender as FloatingCanvasExpandToggleButtonScript;               
                ExpandToggleState = expandToggleButtonScript.State;
                ContentContainer.SetActive(ExpandToggleState);
                break;
        }
    }

    private void FixedUpdate()
    {
        float CurrentActiveValue = ExpandToggleState ? 1f : 0f;
        activeSmooth = (4 * activeSmooth + CurrentActiveValue) / 5;
        panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Lerp(heightRange.x, heightRange.y, activeSmooth));
    }

}


