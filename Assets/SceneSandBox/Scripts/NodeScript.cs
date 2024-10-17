using SimBuilderLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

namespace SceneSandBox
{
    public class NodeScript : MediatorComponent, IUpdateModelComponentNameHandler
    {
        Model Model;
        public State State;
        public Vector2 prevPosition;

        float PositionInitialX = 0.1f;
        float PositionInitialY = 0.9f;
        
        public float positionOffsetIncrement = 0.2f;
        public static float positionOffsetX = 0.0f;
        public static float positionOffsetY = 0.0f;

        public void OnDestroy()
        {
            State.UpdateModelComponentNameEventListener(this, false);
            Model.RemoveState(State);
        }

        public void InitializeComponent(MediatorComponent mediator, Model model, State state)
        {
            base.InitializeComponent(mediator);
            Model = model;
            State = state;
            setPosition();

            GetComponentInChildren<NodeColorScript>().setColor(state.color);
            GetComponentInChildren<NodeDragAreaScript>().InitializeComponent(this);
            GetComponentInChildren<NodeOutScript>().InitializeComponent(this);
            GetComponentInChildren<NodeInScript>().InitializeComponent(this);
            GetComponentInChildren<NodeUpAndOutScript>().InitializeComponent(this);
            GetComponentInChildren<NodeStateTextScript>().SetText(state.Name);
            
            State.UpdateModelComponentNameEventListener(this);

            
        }

        public override void Notify(object sender, params object[] args)
        {
            switch (sender)
            {
                case NodeDragAreaScript:
                    dragStateSciptHandler((NodeDragAreaScript)sender);
                    break;
                default:
                    mediator.Notify(sender, args);
                    break;
            }
        }

        void dragStateSciptHandler(NodeDragAreaScript sender)
        {
            Vector2 newPosition;
            Vector2 offset = new Vector2(0, 0);

            switch (sender.dragState)
            {
                case NodeDragAreaScript.DragState.OnBeginDrag:
                    GetComponent<CanvasGroup>().blocksRaycasts = false;
                    //sender.GetComponent<Image>().raycastTarget = false;
                    prevPosition = (Vector2)sender.eventData.pointerDrag.transform.position;
                    offset = prevPosition - sender.eventData.position;
                    break;
                case NodeDragAreaScript.DragState.OnDrag:
                    newPosition = sender.eventData.position + offset;
                    newPosition = GetClampedMousePosition(newPosition);
                    transform.position = newPosition;
                    gameObject.GetComponentInChildren<NodeOutScript>().NotifyPositionHandler();
                    gameObject.GetComponentInChildren<NodeInScript>().NotifyPositionHandler();
                    gameObject.GetComponentInChildren<NodeUpAndOutScript>().NotifyPositionHandler();
                    break;
                case NodeDragAreaScript.DragState.EndDrag:
                    GetComponent<CanvasGroup>().blocksRaycasts = true;

                    //commandStack.Push(new CommandDrag(this, (DragStateScript)sender));
                    //commandStack.Peek().execute();
                    break;
            }
        }

        public void UpdateModelComponentNameCallback(object sener, string newName)
        {
            GetComponentInChildren<NodeStateTextScript>().SetText(newName);
        }

        public void setPosition()
        {
            GetComponent<RectTransform>().anchorMin = new Vector2(PositionInitialX + positionOffsetX, PositionInitialY - positionOffsetY);
            GetComponent<RectTransform>().anchorMax = new Vector2(PositionInitialX + positionOffsetX, PositionInitialY - positionOffsetY);
            positionOffsetX += 0.1f;

            if (positionOffsetX >= 1.0f)
            {
                positionOffsetX = 0;
                positionOffsetY += 0.1f;
            }

            if (positionOffsetY >= 1.0f)
            {
                positionOffsetY = 0;
                positionOffsetIncrement += 0.01f;
            }
        }

        private Vector3 GetClampedMousePosition(Vector3 currentMousePosition)
        {
            Vector3[] corners = new Vector3[4];
            GetComponentInParent<SandBoxScript>().GetComponent<RectTransform>().GetWorldCorners(corners);
            Vector3 bottomLeft = corners[0];
            Vector3 topRight = corners[2];

            float paddingHorizontal = GetComponent<RectTransform>().rect.width / 2;
            float paddingVertical = GetComponent<RectTransform>().rect.height / 2;

            Vector3 clampedMousePosition = new Vector3();          
            
            clampedMousePosition.x = Mathf.Clamp(currentMousePosition.x, bottomLeft.x + paddingHorizontal, topRight.x - paddingHorizontal);
            clampedMousePosition.y = Mathf.Clamp(currentMousePosition.y, bottomLeft.y + paddingVertical, topRight.y - paddingVertical);


            return clampedMousePosition;
        }

    }

}
