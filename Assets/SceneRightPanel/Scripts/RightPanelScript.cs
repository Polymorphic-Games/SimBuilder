using CanvasScenePlot;
using ScenePlot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneRightPanel
{
    public class RightPanelScript : MediatorComponent
    {
        Model Model;
        [SerializeField] GameObject OptionsPanelPrefab;
        [SerializeField] GameObject PlotPanelPrefab;

        public void InitializeComponent(MediatorComponent mediator, Model model)
        {
            base.InitializeComponent(mediator);
            Model = model;

            Instantiate(OptionsPanelPrefab, transform);
            Instantiate(PlotPanelPrefab, transform);

            GetComponentInChildren<FloatingCanvasOptionsPanelScript>().InitializeComponent(this, model);
            GetComponentInChildren<FloatingCanvasPlotPanelScript>().InitializeComponent(this, model);
        }
    
    }
}