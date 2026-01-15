using System;
using UnityEngine;

[Serializable]
public class GridCell
{
    public GameObject cellBg;
    public int rowIndex;
    public int colIndex;

    public GridCell(GameObject bg, int row, int col)
    {
        cellBg = bg;
        rowIndex = row;
        colIndex = col;
    }
}
