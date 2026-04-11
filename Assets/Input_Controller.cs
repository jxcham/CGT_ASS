using UnityEngine;

public class InputController : MonoBehaviour
{
    public Camera cam;
    public DijkstraPathfinding pathfinding;
    public GridGenerator grid;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Tile clickedTile = hit.collider.GetComponent<Tile>();

                if (clickedTile != null)
                {
                    pathfinding.endTile = clickedTile;
                    pathfinding.FindPath();
                }
            }
        }
    }
}