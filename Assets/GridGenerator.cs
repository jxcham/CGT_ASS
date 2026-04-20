using System;
using Unity.VisualScripting;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public GameObject tilePrefab;

    public Tile[,] grid;

    public DijkstraPathfinding pathfinding;

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        grid = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject tileObj = Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.identity);

                tileObj.transform.parent = transform; 

                Tile tile = tileObj.GetComponent<Tile>();
                grid[x, z] = tile;

                int rand = UnityEngine.Random.Range(0, 4);

                switch (rand)
                {
                    case 0:
                        tile.cost = 1;
                        tile.SetColor(Color.white);
                        break;
                    case 1:
                        tile.cost = 2;
                        tile.SetColor(Color.green);
                        break;
                    case 2:
                        tile.cost = 5;
                        tile.SetColor(Color.blue);
                        break;
                    case 3:
                        tile.cost = 999;
                        tile.isWalkable = false;
                        tile.SetColor(Color.red);
                        break;
                }
            }
        }
    }

    void ClearGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void RegenerateGrid()
    {
        ClearGrid();
        GenerateGrid();

        if (pathfinding != null)
        {
            pathfinding.startTile = null;
            pathfinding.endTile = null;
            pathfinding.finalPath.Clear();

            if (pathfinding.lineRenderer != null)
                pathfinding.lineRenderer.positionCount = 0;
        }
    }
}