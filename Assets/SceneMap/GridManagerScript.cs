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
    private float paddingIncrement = .02f;
    [SerializeField] private GameObject tilePrefab;
    private GameObject tile;
    private List<List<GameObject>> tiles = new();

    public List<List<int>> populationInfected = new();

    public void GenerateGrid(int width, int height)
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
                tile = Instantiate(tilePrefab, new Vector3(x - width / 2 + paddingX, y - height / 2 + paddingY), Quaternion.identity);
                tiles[x].Add(tile);
                //tiles[x][y].GetComponent<TileScript>().model.LoadModel(globalModel);

                paddingY += paddingIncrement;
            }

            paddingY = 0;
            paddingX += paddingIncrement;
        }
    }

    public void colorCell(int i, int j, int population)
    {
        float redValue = (float)population / 10000f;
        tiles[i][j].GetComponent<SpriteRenderer>().color = new UnityEngine.Color((float)redValue, 0, 0, 1);
    }

    public void colorGrid(Model model)
    {
        float redValue;

        int i = 0;
        foreach (var listListStates in model.states)
        {
            int j = 0;
            foreach (var listStates in listListStates)
            {
                foreach (var state in listStates)
                {
                    if (state.Name == "I")
                    {
                        redValue = state.Value / 10000f;
                        tiles[i][j].GetComponent<SpriteRenderer>().color = new UnityEngine.Color((float)redValue, 0, 0, 1);
                    }
                }
                j++;
            }
            i++;
        }
    }
}