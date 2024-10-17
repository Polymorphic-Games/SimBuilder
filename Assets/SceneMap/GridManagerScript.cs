using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class GridManagerScript : MediatorComponent
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    private float paddingX = 0;
    private float paddingY = 0;
    private float paddingIncrement = .2f;
    [SerializeField] private GameObject tilePrefab;
    private GameObject tile;
    private List<List<GameObject>> tiles = new();
    Model[,] model;
    public void GenerateGrid(Model globalModel, int width, int height)
    {
        tiles = new();
        paddingX = 0;
        paddingY = 0;

        this.width = width;
        this.height = height;

        for (int x = 0; x < width; x++)
        {
            tiles.Add(new List<GameObject>());
            for (int y = 0; y < height; y++)
            {
                tile = Instantiate(tilePrefab, new Vector3(x+paddingX, y+paddingY), Quaternion.identity);
                tiles[x].Add(tile);
                tiles[x][y].GetComponent<TileScript>().model.LoadModel(globalModel);

                paddingY += paddingIncrement;
            }

            paddingY = 0;
            paddingX += paddingIncrement;
        }

    }

    public void ColorGrid(List<List<Model>> gridModels)
    {
        float redValue;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                foreach (State state in gridModels[i][j].StateDictionary.Values)
                {
                    if (state.Name == "I")
                    {
                        redValue = state.Value / 10000f;
                        //redValue = 0.01f;
                        tiles[i][j].GetComponent<SpriteRenderer>().color = new UnityEngine.Color((float) redValue, 0 ,0, 1);
                    }
                }
            }
        }
    }


}
