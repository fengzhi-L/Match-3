using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class GameGridSystem : AbstractSystem, IGameGridSystem
{
    private LevelData _levelData;
    
    protected override void OnInit()
    {
        this.GetModel<IUserModel>().currentLevel.Register(level =>
        {
            SpawnGrid();
            FillGridWithCell(CellType.BaseBlock);
        });
    }

    private void Initialize()
    {
        
    }
    
    public void SpawnGrid()
    {
        var currentLevel = this.GetModel<IUserModel>().currentLevel.Value;
        Debug.Log(currentLevel);
        this.GetModel<ILevelModel>().SetLevelData(currentLevel);
        var levelData = this.GetModel<ILevelModel>().currentLevelData;
        var grid = this.GetModel<IGameGridModel>().currentGrid;
        grid.Clear();
        for (var rowIndex = 0; rowIndex < levelData.gridRow; rowIndex++)
        {
            var row = new List<GridCell>();
            for (var colIndex = 0; colIndex < levelData.gridColumn; colIndex++)
            {
                row.Add(new GridCell(rowIndex, colIndex));
            }
            grid.Add(row);
        }
    }

    public void FillGridWithCell(CellType type)
    {
        var grid = this.GetModel<IGameGridModel>().currentGrid;
        foreach (var rowCells in grid)
        {
            foreach (var cell in rowCells)
            {
                cell.cellType = type;
            }
        }
        Debug.Log("13");
        
        this.SendEvent<CellPrefabChangedEvent>();
    }
}
