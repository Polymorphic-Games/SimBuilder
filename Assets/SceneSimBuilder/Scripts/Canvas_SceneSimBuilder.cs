using ChartAndGraph;
using SceneLeftPanel;
using SceneRightPanel;
using SceneSandBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace SceneSimBuilder
{
    public class Canvas_SceneSimBuilder : MediatorComponent
    {
        Model Model = new();

        [SerializeField] GameObject PrefabLeftPanel;
        [SerializeField] GameObject PrefabSandbox;
        [SerializeField] GameObject PrefabRightPanel;
        //[SerializeField] GameObject ControlPanelPrefab;

        private void Awake()
        {
            Instantiate(PrefabLeftPanel, transform);
            Instantiate(PrefabSandbox, transform);
            Instantiate(PrefabRightPanel, transform);
            //Instantiate(ControlPanelPrefab, transform);

            GetComponentInChildren<LeftPanelScript>().InitializeComponent(this, Model);
            GetComponentInChildren<RightPanelScript>().InitializeComponent(this, Model);
            GetComponentInChildren<SandBoxScript>().InitializeComponent(this, Model);

        }

        public override void Notify(object sender, params object[] args)
        {
            switch (sender)
            {
                case PanelButtonVariantResetScript:
                    OnNotificationPanelButtonVariantResetScript();
                    break;
            }
        }

        public void OnNotificationPanelButtonVariantResetScript()
        {
            //reset some UI elements
            GetComponentInChildren<StatesLayoutContainerScript>().colorEnumerator.Reset();
            NodeScript.positionOffsetY = 0.0f;
            NodeScript.positionOffsetX = 0.0f;
            //reset Model
            Model.ResetModel();
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

