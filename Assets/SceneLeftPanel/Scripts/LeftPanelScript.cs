using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SceneLeftPanel
{
    public class LeftPanelScript : MediatorComponent
    {
        Model Model;
        public void InitializeComponent(MediatorComponent mediator, Model model)
        {
            base.InitializeComponent(mediator);
            Model = model;
            GetComponentInChildren<StatesLayoutContainerScript>().InitializeComponent(this, Model);
            GetComponentInChildren<RatesLayoutContainerScript>().InitializeComponent(this, Model);
            GetComponentInChildren<ConnectionsLayoutContainerScript>().InitializeComponent(this, Model);
        }

        private void Update()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

    }
}
