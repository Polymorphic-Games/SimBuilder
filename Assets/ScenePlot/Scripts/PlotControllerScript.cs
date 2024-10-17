using ChartAndGraph;
using ScenePlot;
using SceneSandBox;
using SimBuilderLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace CanvasScenePlot
{
    public class PlotControllerScript : MediatorComponent, IStateHandler, IUpdateModelComponentNameHandler, IUpdateModelStatesHandler, IStepBackwardModelHandler, IStepForwardModelHandler
    {
        public GraphChart Graph;
        public Model Model;
        public Material lineMaterial, fillMaterial;
        public Material pointMaterial;
        Material instancePointMaterial;
        Material instanceLineMaterial;
        double pointSize = 25.0;
        double lineThickness = 6.0;
        bool stertchFill = false;
        ChartAndGraph.MaterialTiling lineTiling = new ChartAndGraph.MaterialTiling(true, 20);
        List<State> States = new();
        float lastX = 0;

        public void InitializeComponent(MediatorComponent mediator, Model model)
        {
            base.InitializeComponent(mediator);
            Model = model;
            Model.AddUpdateModelStatesEventListener(this);
            Model.AddStateEventListener(this);
            Model.StepBackwardModelEventListener(this);
            Model.StepForwardModelEventListener(this);
            Graph = GetComponent<GraphChart>();



        }



        public void UpdateModelStatesCallback(object sender, Dictionary<int, State> states)
        {

            foreach (KeyValuePair<int, State> entry in states)
            {
               Graph.DataSource.AddPointToCategoryRealtime(entry.Value.UniqueID.ToString(), lastX, entry.Value.Value, 1f);
            }
            lastX += 0.1f;
        }

        public void StepBackwardModelCallback(object sender, Dictionary<int, State> states)
        {
            LinkedListNode<float> point;
            foreach (KeyValuePair<int, State> state in states)
            {
                lastX = 0;
                point = state.Value.PopulationCache.First;
                Graph.DataSource.ClearCategory(state.Value.UniqueID.ToString());
                while(point != state.Value.CachePosition)
                {
                    Graph.DataSource.AddPointToCategory(state.Value.UniqueID.ToString(), lastX, point.Value);
                    lastX += 0.1f;
                    point = point.Next;
                }
            }
        }

        public void StepForwardModelCallback(object sender, Dictionary<int, State> states)
        {
            lastX += 0.1f;
            foreach (KeyValuePair<int, State> state in states)
            {
                if (state.Value.CachePosition != null)
                {
                    Graph.DataSource.AddPointToCategory(state.Value.UniqueID.ToString(), lastX, state.Value.CachePosition.Value);
                }
            }
        }

        public void backward()
        {
            //Graph.DataSource.UpdateLastPointInCategoryRealtime
        }

        public void AddStateCallback(object sender, State newState)
        {
            States.Add(newState);
            instanceLineMaterial = Instantiate(lineMaterial, transform);
            instanceLineMaterial.SetColor("_ColorFrom", newState.color);
            instanceLineMaterial.SetColor("_ColorTo", newState.color);
            Graph.DataSource.AddCategory(newState.UniqueID.ToString(), instanceLineMaterial, lineThickness, lineTiling, fillMaterial, stertchFill, null, pointSize);

        }

        public void RemoveStateCallback(object sender, State state)
        {
            Graph.DataSource.RemoveCategory(state.UniqueID.ToString());
            States.Remove(state);
        }

        public void UpdateModelComponentNameCallback(object sender, string newName)
        {
        }

    }
}