using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class UIManager : MonoBehaviour
{
    public UnityEngine.UI.Text tileInfoText;
    public UnityEngine.UI.Text pathInfoText;

    public void ShowTileInfo(Tile tile)
    {

        if (tile == null)
        {
            tileInfoText.text = "Tile Info\nNone";
            return;
        }

        string walkable = tile.isWalkable ? "Yes" : "No";

        tileInfoText.text =
            "Tile Info\n" +
            "Cost: " + tile.cost + "\n" +
            "Walkable: " + walkable;
    }

    public void ShowPathInfo(System.Collections.Generic.List<Tile> path)
    {
        if (path == null || path.Count == 0)
        {
            pathInfoText.text = "Path Info\nNo Path";
            return;
        }

        float totalCost = 0;

        foreach (Tile t in path)
        {
            totalCost += t.cost;
        }

        pathInfoText.text =
            "Path Info\n" +
            "Steps: " + path.Count + "\n" +
            "Total Cost: " + totalCost;
    }
}