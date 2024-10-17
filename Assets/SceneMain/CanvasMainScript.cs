using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneMain
{
    public class CanvasMainScript : MediatorComponent
    {

        [SerializeField] GameObject CanvasMap;
        Model globalModel = new Model();
        List<List<Model>> gridModels = new();
        ModelSpatial gridModel = new();

        private void Start()
        {
            //GetComponentInChildren<ControlPanelScript>().InitializeComponent(this, gridModel);

            Instantiate(CanvasMap, transform);
            globalModel.LoadModel(new SI());
            createSpatialModels(5);
            //GetComponent<TestModelSpeedScript>().test();
            for (int i = 0; i < 10; i++)
            {
                runModels();
            }
        }

        void createSpatialModels(int size)
        {
            CanvasMap.GetComponentInChildren<GridManagerScript>().GenerateGrid(globalModel, size, size);
            int width = size;
            int height = size;

            for (int i = 0; i < width; i++)
            {
                gridModels.Add(new List<Model>());
                for (int j = 0; j < height; j++)
                {
                    gridModels[i].Add(new Model());
                    gridModels[i][j].LoadModel(new SI());
                    foreach (State state in gridModels[i][j].StateDictionary.Values)
                    {
                        gridModel.states.Add(state);
                    }

                }


            }

            foreach(State state in gridModels[2][2].StateDictionary.Values)
            {
                if (state.Name == "I")
                {
                    state.Value = 1000;
                }
            }


            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (j + 1 < height)
                    {
                        setGridConnections(gridModels[i][j], gridModels[i][j+1]);
                    }

                    if (j - 1 >= 0)
                    {
                        setGridConnections(gridModels[i][j], gridModels[i][j-1]);
                    }

                    if (i + 1 < width)
                    {
                        setGridConnections(gridModels[i][j], gridModels[i+1][j]);
                    }
                    if (i -1 >= 0)
                    {
                        setGridConnections(gridModels[i][j], gridModels[i-1][j]);
                    }
                }
            }            

            CanvasMap.GetComponentInChildren<GridManagerScript>().ColorGrid(gridModels);

        }

        void setGridConnections(Model modelA, Model modelB)
        {
            Rate transmissionRate = gridModel.AddRate("t", 0.1f);
            foreach(State stateA in modelA.StateDictionary.Values)
            {
                foreach(State stateB in modelB.StateDictionary.Values)
                {
                    if (stateA.Name == stateB.Name)
                    {
                        Connection transmission = gridModel.AddConnection(stateA, stateB);
                        transmission.RateEquation.AddRateEquationVariable(transmissionRate);
                    }
                }
            }
        }

        void runModels()
        {
            printModel();
            foreach (var models in gridModels)
            {
                foreach(Model model in models)
                {                    
                    model.run_model_tau_leap(0.1f, 1);
                }
            }
            gridModel.run_model(0.1f, 1);
            CanvasMap.GetComponentInChildren<GridManagerScript>().ColorGrid(gridModels);
        }

        void printModel()
        {
            string debug = "";
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    foreach (State state in gridModels[i][j].StateDictionary.Values)
                    {
                        if (state.Name == "I")
                        {
                            debug += state.Value + "\t";
                            //Debug.Log(state.Value);
                        }
                    }
                }
                debug += "\n";
            }
            Debug.Log(debug);
        }



    }
}
