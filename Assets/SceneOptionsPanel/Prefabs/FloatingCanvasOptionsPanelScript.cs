using UnityEngine;

public class FloatingCanvasOptionsPanelScript : FloatingCanvasScript
{
    Model Model;
    public void InitializeComponent(MediatorComponent mediator, Model model)
    {
        base.InitializeComponent(mediator);
        Model = model;
        GetComponentInChildren<PanelButtonVariantLoadScript>().InitializeComponent(this);
        GetComponentInChildren<PanelButtonVariantSaveScript>().InitializeComponent(this);
        GetComponentInChildren<PanelButtonVariantResetScript>().InitializeComponent(this);
        GetComponentInChildren<PanelButtonVariantUndoScript>().InitializeComponent(this);
        GetComponentInChildren<PanelButtonVariantRedoScript>().InitializeComponent(this);

        heightRange = new Vector2(55f, 150f);


        GetComponentInChildren<PanelButtonVariantSaveScript>().SetInteractable(false);
        GetComponentInChildren<PanelButtonVariantUndoScript>().SetInteractable(false);
        GetComponentInChildren<PanelButtonVariantRedoScript>().SetInteractable(false);
    }

    public override void Notify(object sender, params object[] args)
    {
        base.Notify(sender, args);
        switch (sender)
        {
            case PanelButtonVariantResetScript:
                OnNotificationPanelButtonVariantResetScript(sender);
                break;
            case PanelButtonVariantLoadScript:
                OnNotificationPanelButtonVariantLoadScript(sender as PanelButtonVariantLoadScript);
                break;
            default:
                mediator.Notify(sender, args);
                break;
        }
    }

    void OnNotificationPanelButtonVariantResetScript(object sender)
    {
        mediator.Notify(sender);
    }

    void OnNotificationPanelButtonVariantLoadScript(PanelButtonVariantLoadScript panelButtonVariantLoadScript)
    {
        Model.LoadModel(new SI());
    }
}
