using SceneLeftPanel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneRightPanel { 

    public class CanvasHeadScript : MediatorComponent
    {
        Model Model = new();
        private void Awake()
        {
            GetComponentInChildren<RightPanelScript>().InitializeComponent(this, Model);
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
