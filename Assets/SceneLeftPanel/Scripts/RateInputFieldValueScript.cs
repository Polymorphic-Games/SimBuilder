using SceneLeftPanel;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SceneLeftPanel
{
    public class RateInputFieldValueScript : InputFieldScript
    {
        public void InitializeComponent(MediatorComponent mediator, Rate rate)
        {
            base.InitializeComponent(mediator);
            SetTextWithoutNotify(rate.Value.ToString());
        }
    }
}
