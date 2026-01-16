using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class GameGridController : MonoBehaviour, IController
{
    public GameObject cellPrefab;
    public Transform gridParent;
    
    void Start()
    {
        if (cellPrefab == null)
        {
            Debug.LogError("未分配 cellPrefab!");
            return;
        }

        if (gridParent == null)
        {
            gridParent = transform; // 默认自身为父级
        }

        SpawnGrid();
    }

    void SpawnGrid()
    {
        var levelData = this.GetModel<ILevelModel>().currentLevelData;
        var grid = this.GetModel<IGameGridModel>().currentGrid;
        grid.Clear();
        var halfWidth = (levelData.gridColumn - 1) / 2f;
        var halfHeight = (levelData.gridRow - 1) / 2f;

        for (var rowIndex = 0; rowIndex < levelData.gridRow; rowIndex++)
        {
            var row = new List<GridCell>();
            for (var colIndex = 0; colIndex < levelData.gridColumn; colIndex++)
            {
                var cell = Instantiate(cellPrefab, gridParent);

                var x = colIndex - halfWidth;
                var y = rowIndex - halfHeight;

                cell.transform.localPosition = new Vector3(x, y, 0);
                row.Add(new GridCell(cellPrefab, rowIndex, colIndex));
            }
            grid.Add(row);
        }
    }
    
    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}