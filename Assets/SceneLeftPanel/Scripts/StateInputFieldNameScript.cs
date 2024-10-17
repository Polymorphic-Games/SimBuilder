using SceneLeftPanel;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SceneLeftPanel
{
    public class StateInputFieldNameScript : InputFieldScript
    {
        public void InitializeComponent(MediatorComponent mediator, State state)
        {
            base.InitializeComponent(mediator);
            SetTextWithoutNotify(state.Name);
        }
    }

}
