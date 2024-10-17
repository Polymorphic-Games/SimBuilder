using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace SceneSandBox
{
    public class RateEquationScript : MediatorComponent, IRateEquationVariableHandler, IUpdateRateEquationTextHandler, IUpdatePositionHandler
    {
        [SerializeField] GameObject PrefabRateEquationVariable;
        
        public RateEquation RateEquation { get; set; }
        GameObject InstanceRateLine;
        GameObject InstanceRateEquationVariable;
        Dictionary<State, GameObject> ModelStateToGameObjectNode;
        Dictionary<RateEquation, GameObject> ModelRateEquationToGameObject = new();
        Dictionary<RateEquationVariable, GameObject> RateEquationVariableToGameObject = new();
       
        public void OnDestroy()
        {
            RateEquation?.AddRateEquationVariableEventListener(this, false);
            RateEquation?.UpdateRateEquationTextEventListener(this, false);

            if (InstanceRateLine != null)
            {
                InstanceRateLine.GetComponent<PositionHandlerScript>().UpdatePositionEventListener(this, false);
            }
        }

        public void InitializeComponent(MediatorComponent mediator, RateEquation rateEquation, GameObject instanceRateLine, Dictionary<State, GameObject> modelStateToGameObjectNode)
        {
            base.InitializeComponent(mediator);
            transform.GetComponent<DragStateScript>().InitializeComponent(this);

            RateEquation = rateEquation;
            InstanceRateLine = instanceRateLine;
            ModelStateToGameObjectNode = modelStateToGameObjectNode;
            ModelRateEquationToGameObject.Add(RateEquation, gameObject);

            RateEquation.AddRateEquationVariableEventListener(this);
            RateEquation.UpdateRateEquationTextEventListener(this);
            InstanceRateLine.GetComponent<PositionHandlerScript>().UpdatePositionEventListener(this);
            InstanceRateLine.GetComponent<RateLineScript>().NotifyPositionHandler();
            GetComponentInChildren<TextMeshProUGUI>().text = RateEquation.RateEquationText;
        }

        public override void Notify(object sender, params object[] args)
        {
            switch (sender)
            {
                case DragStateScript:
                    DragStateScript dragStateSender = (DragStateScript)sender;
                    switch (dragStateSender.dragState)
                    {
                        case DragStateScript.DragState.OnDrop:
                            if (dragStateSender.eventData.pointerDrag.GetComponent<NodeUpAndOutScript>() != null)
                            {
                                State state = dragStateSender.eventData.pointerDrag.GetComponentInParent<NodeScript>().State;
                                RateEquation.AddRateEquationVariable(state);
                            }
                            break;
                    }
                    break;
                default:
                    mediator.Notify(sender, args);
                    break;
            }
        }

        public void UpdatePositionCallback(object sender, Vector2 newPosition)
        {
            transform.position = newPosition;
            GetComponent<PositionHandlerScript>().UpdatePosition(transform.position);
        }

        public void AddRateEquationVariableCallback(object sender, RateEquationVariable newRateEquationVariable)
        {
            InstanceRateEquationVariable = Instantiate(PrefabRateEquationVariable, transform);
            InstanceRateEquationVariable.GetComponent<RateEquationVariableScript>().InitializeComponent(this, newRateEquationVariable, ModelStateToGameObjectNode, ModelRateEquationToGameObject);
            RateEquationVariableToGameObject.Add(newRateEquationVariable, InstanceRateEquationVariable);
        }

        public void RemoveRateEquationVariableCallback(object sender, RateEquationVariable variable)
        {
            Destroy(RateEquationVariableToGameObject[variable]);
            RateEquationVariableToGameObject.Remove(variable);
        }


        public void UpdateRateEquationTextCallback(object sender, string rateEquationText)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = rateEquationText;
        }

    }
}
