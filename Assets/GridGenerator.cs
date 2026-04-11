using Unity.VisualScripting;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public GameObject tilePrefab;
    public Tile[,] grid;        // 2d array named grid, storing tile(x,y)

    void Start()
    {
        grid = new Tile[width, height];    // define the space

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject tileObj = Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.identity);
                Tile tile = tileObj.GetComponent<Tile>();

                grid[x, z] = tile;          // store into the array

                int rand = Random.Range(0, 4);

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
}