//using OpenCover.Framework.Model;
using SceneLeftPanel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SceneLeftPanel
{
    public class RatesLayoutContainerScript : MediatorComponent, IRateHandler
    {
        Model Model;
        [SerializeField] GameObject prefabRateContainer;
        GameObject instanceRateContainer;
        Dictionary<int, GameObject> rateContainers = new Dictionary<int, GameObject>();

        public void InitializeComponent(MediatorComponent mediator, Model model)
        {
            base.InitializeComponent(mediator);
            Model = model;

            //initialize components
            GetComponentInChildren<RateLayoutContainerAddButtonScript>().InitializeComponent(this);

            //add model listeners
            Model.AddRateEventListener(this);
        }
        public override void Notify(object sender, params object[] args)
        {
            switch (sender)
            {
                case RateLayoutContainerAddButtonScript:
                    if (Model != null)
                    {
                        commandStack.Push(new SimBuilderLibrary.CommandAddRate(Model));
                        commandStack.Peek().Execute();
                    }
                    break;
                default:
                    base.Notify(sender, args);
                    break;
            }
        }

        public void AddRateCallback(object sender, Rate rate)
        {
            instanceRateContainer = Instantiate(prefabRateContainer, transform);
            instanceRateContainer.GetComponent<RateContainerScript>().InitializeComponent(this, Model, rate);
            rateContainers.Add(rate.UniqueID, instanceRateContainer);
            GetComponentInChildren<RateLayoutContainerAddButtonScript>().transform.SetAsLastSibling();
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

        public void RemoveRateCallback(object sender, Rate rate)
        {
            int key = rate.UniqueID;
            Destroy(rateContainers[key]);
            rateContainers.Remove(key);
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

    }
}
