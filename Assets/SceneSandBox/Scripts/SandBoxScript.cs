using SceneSandbox;
using SimBuilderLibrary;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Reflection;

namespace SceneSandBox
{
    public class SandBoxScript : MediatorComponent, IStateHandler, IConnectionHandler
    {
        Model Model;
        [SerializeField] GameObject ControlPanelPrefab;
        [SerializeField] GameObject PrefabConnection;
        [SerializeField] GameObject PrefabRateLine;
        [SerializeField] GameObject PrefabRateModifierLine;
        [SerializeField] GameObject PrefabNode;

        GameObject InstanceConnection;
        GameObject InstanceRateLine;
        GameObject InstanceRateModifierLine;     

        public Dictionary<int, GameObject> Nodes = new();
        Dictionary<State, GameObject> ModelStateToGameObjectNode = new();
        Dictionary<Connection, GameObject> ModelConnectionToGameObjectConnection = new();

        public void InitializeComponent(MediatorComponent mediator, Model model)
        {
            base.InitializeComponent(mediator);
            Instantiate(ControlPanelPrefab, transform);

            Model = model;
            Model.AddStateEventListener(this);
            Model.AddConnectionEventListener(this);

            GetComponentInChildren<ControlPanelScript>().InitializeComponent(this, Model);
            GetComponentInChildren<ButtonTrashScript>().InitializeComponent(this);           

        }

        private Vector3 GetClampedMousePosition()
        {
            Vector3[] corners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(corners);
            Vector3 bottomLeft = corners[0];
            Vector3 topRight = corners[2];
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 clampedMousePosition = new Vector3();
            clampedMousePosition.x = Mathf.Clamp(currentMousePosition.x, bottomLeft.x, topRight.x);
            clampedMousePosition.y = Mathf.Clamp(currentMousePosition.y, bottomLeft.y, topRight.y);
            return clampedMousePosition;
        }

        private Vector3 GetAroundNodeMousePosition()
        {
            Vector3 aroundNodeMousePosition = new Vector3();



            return aroundNodeMousePosition;
        }

        public override void Notify(object sender, params object[] args)
        {
            switch (sender)
            {
                case PointerStateScript:
                    if (args[0] as string == "NodeDragAreaScript")
                    {
                        OnNotificationNodeDragAreaScriptPointerStateScript(sender as PointerStateScript);
                    }
                    break;
                case NodeOutScript:
                    NodeOutScriptNotification((NodeOutScript)sender);
                    break;
                case NodeInScript:
                    NodeInScriptNotification((NodeInScript) sender);
                    break;
                case NodeUpAndOutScript:
                    NodeUpAndOutScriptNotification((NodeUpAndOutScript)sender);
                    break;
                case ButtonTrashScript:
                    OnNotificationButtonTrashScript(sender as ButtonTrashScript);
                    Debug.Log("Trash");
                    break;
            }
        }

        void OnNotificationNodeDragAreaScriptPointerStateScript(PointerStateScript sender)
        {
            if (sender.pointerState == PointerStateScript.PointerState.OnPointerEnter)
            {
                
            }
        }

        void OnNotificationButtonTrashScript(ButtonTrashScript sender)
        {
            
        }

        void NodeOutScriptNotification(NodeOutScript sender)
        {
            DragStateScript.DragState dragState = sender.GetComponentInChildren<DragStateScript>().dragState;
            PointerEventData eventData = sender.GetComponentInChildren<DragStateScript>().eventData;

            switch (dragState)
            {
                case DragStateScript.DragState.OnBeginDrag:
                    InstanceRateLine = Instantiate(PrefabRateLine, transform);
                    InstanceRateLine.GetComponent<RateLineScript>().InitializeComponent(this, sender.gameObject);
                    break;
                case DragStateScript.DragState.OnDrag:
                    InstanceRateLine.GetComponent<RateLineScript>().UpdateEndPosition(GetClampedMousePosition());
                    break;
                case DragStateScript.DragState.EndDrag:
                    Destroy(InstanceRateLine);
                    break;
            }
        }

        void NodeInScriptNotification(NodeInScript sender)
        {
            DragStateScript.DragState dragState = sender.GetComponentInChildren<DragStateScript>().dragState;
            PointerEventData eventData = sender.GetComponentInChildren<DragStateScript>().eventData;
            GameObject onDragGameObject = eventData.pointerDrag.gameObject;

            switch (dragState)
            {
                case DragStateScript.DragState.OnDrop:
                    if (onDragGameObject.GetComponent<NodeOutScript>() != null)
                    {
                        State fromState = onDragGameObject.GetComponentInParent<NodeScript>().State;
                        State toState = sender.GetComponentInParent<NodeScript>().State;
                        Model.AddConnection(fromState, toState);
                    }
                    break;
            }
        }

        void NodeUpAndOutScriptNotification(NodeUpAndOutScript sender)
        {
            DragStateScript.DragState dragState = sender.GetComponentInChildren<DragStateScript>().dragState;
            PointerEventData eventData = sender.GetComponentInChildren<DragStateScript>().eventData;

            switch (dragState)
            {
                case DragStateScript.DragState.OnBeginDrag:
                    InstanceRateModifierLine = Instantiate(PrefabRateModifierLine, transform);
                    InstanceRateModifierLine.GetComponent<RateModifierLineScript>().InitializeComponent(this, sender.gameObject.transform.position);
                    break;
                case DragStateScript.DragState.OnDrag:
                    InstanceRateModifierLine.GetComponent<RateModifierLineScript>().updateEndPosition(GetClampedMousePosition());
                    break;
                case DragStateScript.DragState.EndDrag:
                    Destroy(InstanceRateModifierLine);
                    break;
            }
        }

        public void AddStateCallback(object sender, State newState)
        {
            GameObject instanceNode = Instantiate(PrefabNode, transform);
            instanceNode.GetComponentInChildren<NodeScript>().InitializeComponent(this, Model, newState);
            Nodes.Add(newState.UniqueID, instanceNode);
            ModelStateToGameObjectNode.Add(newState, instanceNode);
        }

        public void RemoveStateCallback(object sender, State state)
        {
            Destroy(Nodes[state.UniqueID]);
            Nodes.Remove(state.UniqueID);
            ModelStateToGameObjectNode.Remove(state);
        }

        public void AddConnectionCallback(object sender, Connection newConnection)
        {
            InstanceConnection = Instantiate(PrefabConnection, transform);           
            InstanceConnection.GetComponent<ConnectionScript>().InitializeComponent(this, newConnection, ModelStateToGameObjectNode);
            ModelConnectionToGameObjectConnection.Add(newConnection, InstanceConnection);
        }

        public void RemoveConnectionCallback(object sender, Connection connection)
        {
            Destroy(ModelConnectionToGameObjectConnection[connection]);
            ModelConnectionToGameObjectConnection.Remove(connection);
        }
    }

}
