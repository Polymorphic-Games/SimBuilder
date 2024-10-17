using SceneLeftPanel;
using SceneSandbox;
using SimBuilderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneSandBox

{
    public class ConnectionScript : MediatorComponent, IUpdateConnectionToStateHandler, IUpdateConnectionFromStateHandler
    {
        public Connection Connection;

        [SerializeField] GameObject PrefabRateLine;
        [SerializeField] GameObject PrefabRateEquation;
        [SerializeField] GameObject PrefabRateModifierLine;
        GameObject FromNode;
        GameObject ToNode;
        GameObject InstanceRateLine { get; set; }
        GameObject InstanceRateEquation;
        Dictionary<State, GameObject> ModelStateToGameObjectNode = new();

        public void OnDestroy()
        {
            Connection?.UpdateFromStateEventListener(this, false);
            Connection?.UpdateToStateEventListener(this, false);
        }

        public void InitializeComponent(MediatorComponent mediator, Connection connection, Dictionary<State, GameObject> modelStateToGameObjectNode)
        {
            base.InitializeComponent(mediator);
            ModelStateToGameObjectNode = modelStateToGameObjectNode;
            Connection = connection;
            Connection.UpdateFromStateEventListener(this);
            Connection.UpdateToStateEventListener(this);
            AddRateLine();
        }

        public void AddRateLine()
        {
            if (Connection.FromState != null)
            {
                FromNode = ModelStateToGameObjectNode[Connection.FromState];
            }

            if (Connection.ToState != null)
            {
                ToNode = ModelStateToGameObjectNode[Connection.ToState];
            }

            if (FromNode != null && ToNode != null)
            {
                GameObject nodeOut = FromNode.GetComponentInChildren<NodeOutScript>().gameObject;
                GameObject nodeIn = ToNode.GetComponentInChildren<NodeInScript>().gameObject;

                InstanceRateLine = Instantiate(PrefabRateLine, transform);
                InstanceRateLine.GetComponent<RateLineScript>().InitializeComponent(this, nodeOut, nodeIn);

                InstanceRateEquation = Instantiate(PrefabRateEquation, transform);
                InstanceRateEquation.GetComponent<RateEquationScript>().InitializeComponent(this, Connection.RateEquation, InstanceRateLine, ModelStateToGameObjectNode);
            }

        }


        public void UpdateFromStateCallback(object sender, State newState)
        {
            if (newState == null)
            {
                return;
            }

            Connection.FromState = newState;

            //remove old listener
            if (FromNode != null)
            {
                FromNode.GetComponentInChildren<NodeOutScript>().GetComponent<PositionHandlerScript>().UpdatePositionEventListener(InstanceRateLine.GetComponent<RateLineScript>(), false);
            }


            if (InstanceRateLine == null)
            {
                AddRateLine();

            } else if (InstanceRateLine != null && newState!=null)
            {
                FromNode = ModelStateToGameObjectNode[newState];
                FromNode.GetComponentInChildren<NodeOutScript>().GetComponent<PositionHandlerScript>().UpdatePositionEventListener(InstanceRateLine.GetComponent<RateLineScript>());
                FromNode.GetComponentInChildren<NodeOutScript>().GetComponent<PositionHandlerScript>().BroadcastCurrentPosition();
            }



        }
                
        public void UpdateToStateCallback(object sender, State newState)
        {

            Connection.ToState = newState;

            //remove old listener
            if (ToNode != null && InstanceRateLine != null)
            {
                ToNode.GetComponentInChildren<NodeInScript>().GetComponent<PositionHandlerScript>().UpdatePositionEventListener(InstanceRateLine.GetComponent<RateLineScript>(), false);
            }


            if (newState == null)
            {
                Destroy(InstanceRateLine);
                Destroy(InstanceRateEquation);
                //InstanceRateEquation.SetActive(false);
                return;
            }

            //if (InstanceRateEquation != null)
            //{
            //    InstanceRateEquation.SetActive(true);
            //}

            if (InstanceRateLine != null)
            {
                //InstanceRateLine.SetActive(true);
                ToNode = ModelStateToGameObjectNode[newState];
                ToNode.GetComponentInChildren<NodeInScript>().GetComponent<PositionHandlerScript>().UpdatePositionEventListener(InstanceRateLine.GetComponent<RateLineScript>());
                ToNode.GetComponentInChildren<NodeInScript>().GetComponent<PositionHandlerScript>().BroadcastCurrentPosition();
            }
            else
            {
                AddRateLine();
            } 
        }


    }

}
