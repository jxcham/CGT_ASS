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

    void Update()
    {
        // optional: auto update start tile position
        if (player != null)
        {
            startTile = GetTileFromWorld(player.position);
        }
    }

    // CALL THIS WHEN YOU CLICK DESTINATION
    public void FindPath()
    {
        if (startTile == null || endTile == null) return;

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

            // Reached destination
            if (current == endTile)
            {
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
    }

    // -----------------------------
    // GET LOWEST COST NODE
    // -----------------------------
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

    // -----------------------------
    // GET NEIGHBORS (4 direction)
    // -----------------------------
    List<Tile> GetNeighbors(Tile tile)
    {
        List<Tile> neighbors = new List<Tile>();

        Vector2Int pos = GetTilePosition(tile);
        int x = pos.x;
        int z = pos.y;

        TryAddNeighbor(x + 1, z, neighbors);
        TryAddNeighbor(x - 1, z, neighbors);
        TryAddNeighbor(x, z + 1, neighbors);
        TryAddNeighbor(x, z - 1, neighbors);

        return neighbors;
    }

    void TryAddNeighbor(int x, int z, List<Tile> list)
    {
        if (x < 0 || x >= gridGenerator.width ||
            z < 0 || z >= gridGenerator.height)
            return;

        list.Add(gridGenerator.grid[x, z]);
    }

    // -----------------------------
    // CONVERT TILE to GRID POSITION
    // -----------------------------
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

    // -----------------------------
    // CONVERT WORLD to TILE
    // -----------------------------
    Tile GetTileFromWorld(Vector3 pos)
    {
        int x = Mathf.RoundToInt(pos.x);
        int z = Mathf.RoundToInt(pos.z);

        if (x < 0 || x >= gridGenerator.width ||
            z < 0 || z >= gridGenerator.height)
            return null;

        return gridGenerator.grid[x, z];
    }

    // -----------------------------
    // REBUILD PATH
    // -----------------------------
    void RetracePath()
    {
        Tile current = endTile;

        while (current != startTile)
        {
            if (current != startTile && current != endTile)
                current.SetColor(Color.yellow);

            current = current.parent;
        }

        startTile.SetColor(Color.green);
        endTile.SetColor(Color.red);
    }

    // -----------------------------
    // RESET GRID DATA
    // -----------------------------
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
}