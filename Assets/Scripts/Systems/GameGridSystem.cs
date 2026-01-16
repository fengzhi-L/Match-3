using System.Collections.Generic;
using QFramework;

public class GameGridSystem : AbstractSystem, IGameGridSystem
{
    private LevelData _levelData;
    
    protected override void OnInit()
    {
        
    }

    private void Initialize()
    {
        
    }
    
    public void SpawnGrid()
    {
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
        
        this.SendEvent<CellPrefabChangedEvent>();
    }
}
