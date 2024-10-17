using ChartAndGraph;
using SimBuilderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace SceneLeftPanel
{
    public class CanvasHeadScript : MediatorComponent
    {
        Model Model = new();
        private void Awake()
        {
            GetComponentInChildren<LeftPanelScript>().InitializeComponent(this, Model);
            GetComponentInChildren<Button_RunModel_Script>().InitializeComponent(this, Model);
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
