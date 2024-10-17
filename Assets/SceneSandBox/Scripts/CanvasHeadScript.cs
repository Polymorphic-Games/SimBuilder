using ChartAndGraph;
using SceneLeftPanel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace SceneSandBox
{
    public class CanvasHeadScript : MediatorComponent
    {

        Model Model = new();
        private void Awake()
        {
            GetComponentInChildren<SandBoxScript>().InitializeComponent(this, Model);

            GetComponentInChildren<ButtonAddStateScript>().InitializeComponent(this);
        }

        public override void Notify(object sender, params object[] args)
        {
            switch (sender)
            {
                case ButtonAddStateScript:
                    if (Model != null)
                    {
                        commandStack.Push(new SimBuilderLibrary.CommandAddState(Model));
                        commandStack.Peek().Execute();
                    }
                    break;
                default:
                    base.Notify(sender, args);
                    break;
            }
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
