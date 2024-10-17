using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using SimBuilderLibrary;

namespace SceneLeftPanel
{
    public class RateEquationContainerScript : MediatorComponent, IRateEquationVariableHandler
    {
        Model Model;
        RateEquation RateEquation;

        [SerializeField] GameObject PrefabRateEquationVariable;
        GameObject InstanceRateEquaitonVariable;
        Dictionary<RateEquationVariable, GameObject> RateEquationVariableToGameObject = new();

        public void OnDestroy()
        {
            RateEquation.AddRateEquationVariableEventListener(this, false);
        }

        public void InitializeComponent(MediatorComponent mediator, Model model, RateEquation rateEquation)
        {
            base.InitializeComponent(mediator);
            Model = model;
            RateEquation = rateEquation;
           
            GetComponentInChildren<RateEquationAddVarButtonScript>().InitializeComponent(this);
            GetComponentInChildren<RateEquationFromStateScript>().setValue(RateEquation?.FromState?.Name);

            RateEquation.AddRateEquationVariableEventListener(this);

            //foreach (var rateEquationVariable in rateEquation.RateEquationVariables)
            //{
            //    AddRateEquationVariableCallback(rateEquationVariable, rateEquationVariable);
            //}

        }

        public override void Notify(object sender, params object[] args)
        {
            switch (sender)
            {
                case RateEquationAddVarButtonScript:
                    commandStack.Push(new CommandAddRateEquationVar(RateEquation));
                    commandStack.Peek().Execute();
                    break;
                default:
                    base.Notify(sender, args);
                    break;
            }
        }


        public void AddRateEquationVariableCallback(object sender, RateEquationVariable variable)
        {
            InstanceRateEquaitonVariable = Instantiate(PrefabRateEquationVariable, transform);
            InstanceRateEquaitonVariable.GetComponentInChildren<RateEquationVarDropdownScript>().InitializeComponent(this, Model, variable);
            RateEquationVariableToGameObject.Add(variable, InstanceRateEquaitonVariable);
            GetComponentInChildren<RateEquationAddVarButtonScript>().transform.SetAsLastSibling();
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

        public void RemoveRateEquationVariableCallback(object sender, RateEquationVariable variable)
        {
            Destroy(RateEquationVariableToGameObject[variable]);
            RateEquationVariableToGameObject.Remove(variable);
        }



    }


}

