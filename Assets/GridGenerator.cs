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
    public Camera cam;
    public Transform player;

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        ClearGrid();

        grid = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject tileObj = Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
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
        AdjustCamera();
    }

    void ClearGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    void ResetPathData()
    {
        foreach (Transform child in transform)
        {
            Tile t = child.GetComponent<Tile>();
            if (t != null)
            {
                t.parent = null;
                t.gCost = Mathf.Infinity;
            }
        }
    }

    public void RegenerateGrid()
    {
        transform.position = Vector3.zero;

        if (pathfinding != null)
        {
            pathfinding.startTile = null;
            pathfinding.endTile = null;
            pathfinding.finalPath.Clear();

            if (pathfinding.lineRenderer != null)
            {
                pathfinding.lineRenderer.positionCount = 0;
            }
        }

        GenerateGrid();
        AdjustCamera();
        ResetPlayer();
    }

    void AdjustCamera()
    {
        if (cam == null) return;

        float centerX = (width - 1) / 2f;
        float centerZ = (height - 1) / 2f;

        float size = Mathf.Max(width, height);

        cam.transform.position = new Vector3(centerX, size * 1.2f, centerZ);
        cam.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        cam.orthographic = true;

        cam.orthographicSize = size / 2f + 2f;
    }

    void ResetPlayer()
    {
        if (player == null) return;

        int startX = 0;
        int startZ = 0;

        Tile startTile = grid[startX, startZ];

        if (startTile != null)
        {
            player.position = new Vector3(
                startTile.transform.position.x,
                1f,
                startTile.transform.position.z
            );
        }
    }
}