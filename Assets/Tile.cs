using UnityEngine;

public class Tile : MonoBehaviour
{
    public int cost = 1;
    public bool isWalkable = true;
    public float gCost = Mathf.Infinity;   
    public Tile parent;
    public void SetColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }
}