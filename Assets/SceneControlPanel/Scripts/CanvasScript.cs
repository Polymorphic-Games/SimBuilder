using ChartAndGraph;
using SimBuilderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace SceneControlPanel
{
    public class CanvasScript : MediatorComponent
    {
        [SerializeField] GameObject ControlPanelPrefab;

        Model Model = new();
        private void Awake()
        {
            Instantiate(ControlPanelPrefab, transform);
            GetComponentInChildren<ControlPanelScript>().InitializeComponent(this, Model);
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
