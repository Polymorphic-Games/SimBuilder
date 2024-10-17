using SceneSandbox;
using SimBuilderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneSandBox
{
    public class RateEquationVariableScript : MediatorComponent, IUpdateRateEquationVariableModelComponentHandler
    {
        [SerializeField] GameObject PrefabRateModifierLine;

        public RateEquationVariable RateEquationVariable { get; set; }
        RateEquation RateEquation;
        Dictionary<State, GameObject> ModelStateToGameObjectNode;
        Dictionary<RateEquation, GameObject> ModelRateEquationToGameObject;

        GameObject StateNode;
        GameObject NodeUpAndOut;
        GameObject RateEquationGameObject;
        GameObject InstanceRateModifierLine;


        private void OnDestroy()
        {
            RateEquationVariable.UpdateRateEquationVariableModelComponentEventListener(this, false);
        }
        public void InitializeComponent(MediatorComponent mediator, RateEquationVariable rateEquationVariable, Dictionary<State, GameObject> modelStateToGameObjectNode, Dictionary<RateEquation, GameObject> modelRateEquationToGameObject)
        {
            base.InitializeComponent(mediator);
            RateEquationVariable = rateEquationVariable;
            ModelStateToGameObjectNode = modelStateToGameObjectNode;
            ModelRateEquationToGameObject = modelRateEquationToGameObject;
            RateEquation = RateEquationVariable.RateEquation;
            RateEquationGameObject = ModelRateEquationToGameObject[RateEquation];


            if (RateEquationVariable.ModelComponent != null && RateEquationVariable.ModelComponent.IsState)
            {
                State state = (State)RateEquationVariable.ModelComponent;
                StateNode = ModelStateToGameObjectNode[state];
                NodeUpAndOut = StateNode.GetComponentInChildren<NodeUpAndOutScript>().gameObject;
                AddRateModifierLine(RateEquationVariable.ModelComponent);
            }


            RateEquationVariable.UpdateRateEquationVariableModelComponentEventListener(this);
        }

        public void AddRateModifierLine(ModelComponent modelComponent)
        {
            if (modelComponent != null && modelComponent.IsState)
            {
                InstanceRateModifierLine = Instantiate(PrefabRateModifierLine, transform);
                InstanceRateModifierLine.GetComponent<RateModifierLineScript>().InitializeComponent(this, RateEquationGameObject, NodeUpAndOut);
            }
        }


        public void UpdateRateEquationVariableModelComponentCallback(object sender, ModelComponent newModelComponent)
        {
            Destroy(InstanceRateModifierLine);
            if (newModelComponent == null)
            {
                return;
            }
            else if (newModelComponent.IsState)
            {
                State state = (State)newModelComponent;
                StateNode = ModelStateToGameObjectNode[state];
                NodeUpAndOut = StateNode.GetComponentInChildren<NodeUpAndOutScript>().gameObject;
                AddRateModifierLine(newModelComponent);
            }
        }
    }
}
