//using OpenCover.Framework.Model;
using SceneLeftPanel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
//using static ICommand;

namespace SceneLeftPanel {
    public class StatesLayoutContainerScript : MediatorComponent, IStateHandler
    {
        Model Model;
        [SerializeField] GameObject prefabStateContainer;
        GameObject instanceStateContainer;
        Dictionary<int, GameObject> stateContainers = new Dictionary<int, GameObject>();
        Color32[] colors;
        public IEnumerator colorEnumerator;

        public void InitializeComponent(MediatorComponent mediator, Model model)
        {
            base.InitializeComponent(mediator);
            Model = model;
            colors = new Color32[] {
                new Color32(236, 122, 121, 255),
                new Color32(81, 194, 162, 255),
                new Color32(212, 140, 98, 255),
                new Color32(177, 164, 62, 255),
                new Color32(94, 179, 208, 255),
                new Color32(130, 153, 245, 255),
                new Color32(139, 184, 72, 255),
                new Color32(185, 128, 241, 255),
                new Color32(90, 204, 91, 255),
                new Color32(226, 112, 205, 255)
            };
            colorEnumerator = colors.GetEnumerator();

            //initialize components
            GetComponentInChildren<StateLayoutContainerAddButtonScript>().InitializeComponent(this);

            //add model listeners
            Model.AddStateEventListener(this);
        }

        public override void Notify(object sender, params object[] args)
        {
            switch (sender)
            {
                case StateLayoutContainerAddButtonScript:
                    if (Model != null)
                    {
                        commandStack.Push(new SimBuilderLibrary.CommandAddState(Model));
                        commandStack.Peek().Execute();
                    }
                    break;
                default:
                    base.Notify(sender, args);
                    break;
            }
        }

        public void AddStateCallback(object sender, State state)
        {
            state.color = getColor();
            instanceStateContainer = Instantiate(prefabStateContainer, transform);
            instanceStateContainer.GetComponent<StateContainerScript>().InitializeComponent(this, Model, state);
            stateContainers.Add(state.UniqueID, instanceStateContainer);
            GetComponentInChildren<StateLayoutContainerAddButtonScript>().transform.SetAsLastSibling();
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

        public void RemoveStateCallback(object sender, State state)
        {
            int key = state.UniqueID;
            Destroy(stateContainers[key]);
            stateContainers.Remove(key);
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

        Color32 getColor()
        {
            if (colorEnumerator.MoveNext() == false)
            {
                colorEnumerator.Reset();
                colorEnumerator.MoveNext();
            }
            return (Color32)colorEnumerator.Current;
        }

    }
}
