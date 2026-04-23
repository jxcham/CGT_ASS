using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraPathfinding : MonoBehaviour
{
    public GridGenerator gridGenerator;
    public Transform player;

    public Tile startTile;
    public Tile endTile;

    private List<Tile> openList = new List<Tile>();
    private List<Tile> closedList = new List<Tile>();

    public List<Tile> finalPath = new List<Tile>();

    public LineRenderer lineRenderer;
    public UIManager uiManager;
   
    void Update()
    {
        if (player != null)
        {
            startTile = GetTileFromWorld(player.position);
        }
    }

    public void FindPath()
    {
        if (startTile == null || endTile == null) return;

        float startTime = Time.realtimeSinceStartup;

        ResetGrid();

        openList.Clear();
        closedList.Clear();

        startTile.gCost = 0;
        openList.Add(startTile);

        while (openList.Count > 0)
        {
            Tile current = GetLowestCostTile(openList);

            openList.Remove(current);
            closedList.Add(current);

            if (current == endTile)
            {
                float endTime = Time.realtimeSinceStartup;
                float pathTime = endTime - startTime;

                UnityEngine.Debug.Log("Pathfinding Time (SUCCESS): " + pathTime + "s");

                RetracePath();
                return;
            }

            foreach (Tile neighbor in GetNeighbors(current))
            {
                if (!neighbor.isWalkable || closedList.Contains(neighbor))
                    continue;

                float newCost = current.gCost + neighbor.cost;

                if (!openList.Contains(neighbor) || newCost < neighbor.gCost)
                {
                    neighbor.gCost = newCost;
                    neighbor.parent = current;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        float failEndTime = Time.realtimeSinceStartup;
        float failTime = failEndTime - startTime;

        UnityEngine.Debug.Log("Pathfinding Time (NO PATH): " + failTime + "s");

        OnPathNotFound();
    }

    void OnPathNotFound()
    {
        finalPath.Clear();

        if (lineRenderer != null)
            lineRenderer.positionCount = 0;

        if (uiManager != null)
            uiManager.ShowPathInfo(null);
    }

    Tile GetLowestCostTile(List<Tile> list)
    {
        Tile best = list[0];

        foreach (Tile t in list)
        {
            if (t.gCost < best.gCost)
                best = t;
        }

        return best;
    }

    List<Tile> GetNeighbors(Tile tile)
    {
        List<Tile> neighbors = new List<Tile>();

        Vector2Int pos = GetTilePosition(tile);

        int x = pos.x;
        int z = pos.y;

        TryAdd(x + 1, z, neighbors);
        TryAdd(x - 1, z, neighbors);
        TryAdd(x, z + 1, neighbors);
        TryAdd(x, z - 1, neighbors);

        return neighbors;
    }

    void TryAdd(int x, int z, List<Tile> list)
    {
        if (x < 0 || x >= gridGenerator.width ||
            z < 0 || z >= gridGenerator.height)
            return;

        list.Add(gridGenerator.grid[x, z]);
    }

    Vector2Int GetTilePosition(Tile tile)
    {
        for (int x = 0; x < gridGenerator.width; x++)
        {
            for (int z = 0; z < gridGenerator.height; z++)
            {
                if (gridGenerator.grid[x, z] == tile)
                    return new Vector2Int(x, z);
            }
        }
        return Vector2Int.zero;
    }

    Tile GetTileFromWorld(Vector3 pos)
    {
        if (gridGenerator == null || gridGenerator.grid == null)
            return null;

        int x = Mathf.RoundToInt(pos.x);
        int z = Mathf.RoundToInt(pos.z);

        if (x < 0 || x >= gridGenerator.width ||
            z < 0 || z >= gridGenerator.height)
            return null;

        return gridGenerator.grid[x, z];
    }

    void RetracePath()
    {
        finalPath.Clear();

        Tile current = endTile;

        while (current != startTile)
        {
            finalPath.Add(current);
            current = current.parent;
        }

        finalPath.Reverse();
        DrawPath();

        uiManager.ShowPathInfo(finalPath); 

        StopAllCoroutines();
        StartCoroutine(MovePlayer());
    }

    IEnumerator MovePlayer()
    {
        foreach (Tile t in finalPath)
        {
            Vector3 targetPos = new Vector3(t.transform.position.x, 1, t.transform.position.z);

            while (Vector3.Distance(player.position, targetPos) > 0.05f)
            {
                player.position = Vector3.MoveTowards(player.position, targetPos, 5f * Time.deltaTime);
                yield return null;
            }
        }
    }

    void ResetGrid()
    {
        for (int x = 0; x < gridGenerator.width; x++)
        {
            for (int z = 0; z < gridGenerator.height; z++)
            {
                Tile t = gridGenerator.grid[x, z];
                t.gCost = Mathf.Infinity;
                t.parent = null;
            }
        }
    }

    void DrawPath()
    {
        if (finalPath == null || finalPath.Count == 0)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        lineRenderer.positionCount = finalPath.Count;

        for (int i = 0; i < finalPath.Count; i++)
        {
            Vector3 pos = finalPath[i].transform.position;
            lineRenderer.SetPosition(i, new Vector3(pos.x, 0.2f, pos.z));
        }
    }
}