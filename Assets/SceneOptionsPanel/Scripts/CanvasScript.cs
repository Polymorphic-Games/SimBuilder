using ChartAndGraph;
using SimBuilderLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using Unity.VisualScripting;

namespace SceneOptionsPanel
{
    public class CanvasScript : MediatorComponent
    {
        Model Model = new();

        [SerializeField] GameObject FloatingCanvasPrefab;

        private void Awake()
        {
            Instantiate(FloatingCanvasPrefab, transform);
            GetComponentInChildren<FloatingCanvasOptionsPanelScript>().InitializeComponent(this);
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
