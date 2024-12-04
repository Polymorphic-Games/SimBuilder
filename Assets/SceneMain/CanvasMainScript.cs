using SimBuilderLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace SceneMain
{
    public class CanvasMainScript : MediatorComponent
    {
        [SerializeField] private GameObject CanvasMap;

        //Model globalModel = new Model();
        //List<List<Model>> gridModels = new();
        //ModelSpatial gridModel = new();
        private int gridSize = 21;

        private void Start()
        {
            ModelCombination modelCombination = new ModelCombination();
            Model model = modelCombination.createPopulationSpatialModel(new SI(), gridSize);
            model.states[10][10][1].Value = 1000;

            Instantiate(CanvasMap, transform);
            CanvasMap.GetComponent<GridManagerScript>().GenerateGrid(gridSize, gridSize);
            CanvasMap.GetComponent<GridManagerScript>().colorGrid(model);

            GetComponent<SSAfast>().run(CanvasMap, 1, model);
        }
    }
}