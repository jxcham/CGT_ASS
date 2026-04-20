using System.Collections;
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

    public int seed = 12345;

    void Start()
    {
        // Generate only once per Play session
        if (grid == null)
        {
            GenerateGrid();
        }
    }

    public void GenerateGrid()
    {
        ClearGrid();

        grid = new Tile[width, height];

        // deterministic randomness based on seed
        UnityEngine.Random.InitState(seed);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject tileObj = Instantiate(
                    tilePrefab,
                    new Vector3(x, 0, z),
                    Quaternion.identity,
                    transform
                );

                Tile tile = tileObj.GetComponent<Tile>();
                grid[x, z] = tile;

                int rand = UnityEngine.Random.Range(0, 4);

                switch (rand)
                {
                    case 0:
                        tile.cost = 1;
                        tile.isWalkable = true;
                        tile.SetColor(Color.white);
                        break;

                    case 1:
                        tile.cost = 2;
                        tile.isWalkable = true;
                        tile.SetColor(Color.green);
                        break;

                    case 2:
                        tile.cost = 5;
                        tile.isWalkable = true;
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
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        grid = null;
    }

    public void RegenerateGrid()
    {
        // change seed → new map
        seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);

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

        StartCoroutine(RegenerateRoutine());
    }

    IEnumerator RegenerateRoutine()
    {
        ClearGrid();

        yield return null; // wait for destroy

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
        if (player == null || grid == null) return;

        Tile startTile = grid[0, 0];

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