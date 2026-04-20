using UnityEngine;
using TMPro;

public class GridUI : MonoBehaviour
{
    public GridGenerator grid;

    public TMP_Dropdown dropdown;

    public void OnGridSizeChanged()
    {
        switch (dropdown.value)
        {
            case 0:
                grid.width = 10;
                grid.height = 10;
                break;

            case 1:
                grid.width = 20;
                grid.height = 20;
                break;

            case 2:
                grid.width = 30;
                grid.height = 30;
                break;
        }
    }

    public void OnRegenerateButton()
    {
        OnGridSizeChanged();
        grid.RegenerateGrid();
    }
}