using CanvasScenePlot;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ScenePlot
{
    public class FloatingCanvasPlotPanelScript : FloatingCanvasScript
    {
        Model Model;
        public void InitializeComponent(MediatorComponent mediator, Model model)
        {
            Model = model;
            base.InitializeComponent(mediator);
            GetComponentInChildren<PlotControllerScript>().InitializeComponent(this, model);
            GetComponentInChildren<FloatingCanvasExpandToggleButtonScript>().InitializeComponent(this);
            GetComponentInChildren<RefreshButtonScript>().InitializeComponent(this);

            heightRange = new Vector2(55f, 1000f);
        }

        public override void Notify(object sender, params object[] args)
        {
            switch (sender)
            {
                case RefreshButtonScript:
                    OnNotificationRefreshButtonScript(sender);
                    break;
            }
        }

        void OnNotificationRefreshButtonScript(object sender)
        {
            foreach (State state in Model.StateDictionary.Values)
            {
                GetComponentInChildren<PlotControllerScript>().Graph.DataSource.ClearCategory(state.UniqueID.ToString());
            }
        }
    }

}
