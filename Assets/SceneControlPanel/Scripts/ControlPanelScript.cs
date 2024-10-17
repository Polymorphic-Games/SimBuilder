using SceneControlPanel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanelScript : MediatorComponent
{
    Model Model;
    float PlaySpeed = 1f;
    public void InitializeComponent(MediatorComponent mediator, Model model)
    {
        base.InitializeComponent(mediator);
        Model = model;
        GetComponentInChildren<ToggleButtonVariantPlayScript>().InitializeComponent(this);
        GetComponentInChildren<ButtonVariantForwardScript>().InitializeComponent(this, Model);
        GetComponentInChildren<ButtonVariantBackwardScript>().InitializeComponent(this, Model);
        GetComponentInChildren<ButtonVariantFastScript>().InitializeComponent(this);
        GetComponentInChildren<ButtonVariantSlowScript>().InitializeComponent(this);

        //GetComponentInChildren<ButtonVariantFastScript>().SetInteractable(false);
        //GetComponentInChildren<ButtonVariantSlowScript>().SetInteractable(false);

    }

    public override void Notify(object sender, params object[] args)
    {
        switch (sender)
        {
            case ToggleButtonVariantPlayScript:
                OnNotificationButtonPlayScript(sender as ToggleButtonVariantPlayScript);
                break;
            case ButtonVariantFastScript:
                OnNotificationButtonVariantFastScript();
                break;
            case ButtonVariantSlowScript:
                OnNotificationButtonVariantSlowScript();
                break;
        }
    }

    void OnNotificationButtonPlayScript(ToggleButtonVariantPlayScript buttonVariantPlayScript)
    {
        if (buttonVariantPlayScript.State)
        {
            CancelInvoke("PlayModel");
            GetComponentInChildren<ButtonVariantForwardScript>().SetInteractable(true);
            GetComponentInChildren<ButtonVariantBackwardScript>().SetInteractable(true);


        }
        else
        {
            InvokeRepeating("PlayModel", 0.0f, 1.0f);
            GetComponentInChildren<ButtonVariantForwardScript>().SetInteractable(false);
            GetComponentInChildren<ButtonVariantBackwardScript>().SetInteractable(false);
        }

    }


    void OnNotificationButtonVariantFastScript()
    {
        UpdatePlaySpeed(-.2f);        
        CancelInvoke("PlayModel");
        InvokeRepeating("PlayModel", PlaySpeed, PlaySpeed);
    }

    void OnNotificationButtonVariantSlowScript()
    {
        UpdatePlaySpeed(+.2f);
        CancelInvoke("PlayModel");
        InvokeRepeating("PlayModel", PlaySpeed, PlaySpeed);
    }

    void PlayModel()
    {
        Model.run_model_tau_leap(0.1f, 1);
    }

    void UpdatePlaySpeed(float increment)
    {
        PlaySpeed = Mathf.Clamp(PlaySpeed + increment, 0.1f, 5f);
    }

}
