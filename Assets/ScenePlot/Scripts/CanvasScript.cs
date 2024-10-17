using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ScenePlot
{

    public class CanvasScript : MediatorComponent
    {
        [SerializeField] GameObject PlotPanelPrefab;

        Model Model = new();

        private void Awake()
        {
            Instantiate(PlotPanelPrefab, transform);
            GetComponentInChildren<FloatingCanvasPlotPanelScript>().InitializeComponent(this);
            
        }


        private void OnGUI()
        {
            if (Event.current.Equals(Event.KeyboardEvent("^z")))
            {
                if (commandStack.Count != 0)
                {
                    commandStack.Peek().Undo();
                    commandStack.Pop();
                }

            }
        }
    }



}
