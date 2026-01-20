using System;
using UnityEngine;

[Serializable]
public class GridCell
{
    public CellType cellType;
    public int rowIndex;
    public int colIndex;
    
    public GridCell()
    {
        // 可以设置默认值
        cellType = CellType.BaseBlock;
        rowIndex = 0;
        colIndex = 0;
    }

    public GridCell(CellType type, int row, int col)
    {
        cellType = type;
        rowIndex = row;
        colIndex = col;
    }

    public GridCell(int row, int col)
    {
        rowIndex = row;
        colIndex = col;
    }
}
