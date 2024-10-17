using SceneLeftPanel;
using SimBuilderLibrary;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SceneLeftPanel
{
    public class ConnectionsLayoutContainerScript : MediatorComponent, IConnectionHandler
    {
        Model Model;
        [SerializeField] GameObject prefabConnectionsContainer;
        GameObject instanceConnectionContainer;
        Dictionary<int, GameObject> connectionContainers = new();

        public void InitializeComponent(MediatorComponent mediator, Model model)
        {
            base.InitializeComponent(mediator);
            Model = model;

            //initialize components
            GetComponentInChildren<ConnectionsLayoutContainerAddButtonScript>().InitializeComponent(this);

            //add model listeners
            Model.AddConnectionEventListener(this);
        }

        public override void Notify(object sender, params object[] args)
        {
            switch (sender)
            {
                case ConnectionsLayoutContainerAddButtonScript:
                    if (Model != null)
                    {
                        commandStack.Push(new CommandAddConnection(Model));
                        commandStack.Peek().Execute();
                    }
                    break;
                default:
                    base.Notify(sender, args);
                    break;
            }
        }

        public void AddConnectionCallback(object sender, Connection connection)
        {
            instanceConnectionContainer = Instantiate(prefabConnectionsContainer, transform);
            instanceConnectionContainer.GetComponent<ConnectionContainerScript>().InitializeComponent(this, Model, connection);
            connectionContainers.Add(connection.UniqueID, instanceConnectionContainer);
            GetComponentInChildren<ConnectionsLayoutContainerAddButtonScript>().transform.SetAsLastSibling();
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

        public void RemoveConnectionCallback(object sender, Connection connection)
        {
            Destroy(connectionContainers[connection.UniqueID]);
            connectionContainers.Remove(connection.UniqueID);
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

    }
}
