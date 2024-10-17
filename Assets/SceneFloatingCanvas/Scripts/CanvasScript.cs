using ChartAndGraph;
using SimBuilderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace SceneFloatingCanvas
{
    public class CanvasScript : MediatorComponent
    {

        [SerializeField] GameObject FloatingCanvasPrefab;
        GameObject FloatingCanvasInstance;

        private void Awake()
        {
            Instantiate(FloatingCanvasPrefab, transform);
            GetComponentInChildren<FloatingCanvasScript>().InitializeComponent(this);
        }

        private void OnGUI()
        {
            if (Event.current.Equals(Event.KeyboardEvent("^z")))
            {
                if (commandStack.Count != 0)
                {
                    commandStack.Peek().Undo();
                    commandStack.Pop();
                }
            }
        }

    }
}
