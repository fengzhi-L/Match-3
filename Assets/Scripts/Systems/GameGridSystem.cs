using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class GameGridSystem : AbstractSystem, IGameGridSystem
{
    private LevelData _levelData;
    
    protected override void OnInit()
    {
        var fruitGrid = this.GetModel<IGameGridModel>().currentFruitGrid;
        this.GetModel<IUserModel>().currentLevel.Register(level =>
        {
            SpawnGrid();
            FillGridWithCell(CellType.BaseBlock);
            SpawnFruitGrid();
            // 简单方案：重新生成棋盘
            while (DetectAllMatches().Count > 0)
            {
                Debug.Log("重新生成水果棋盘");
                fruitGrid.Clear();
                SpawnFruitGrid();
            }
        });
    }

    private void Initialize()
    {
        
    }
    
    public void SpawnGrid()
    {
        var currentLevel = this.GetModel<IUserModel>().currentLevel.Value;
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
    
    /// <summary>
    /// 生成多行多列水果
    /// </summary>
    private void SpawnFruitGrid()
    {
        var levelData = this.GetModel<ILevelModel>().currentLevelData;
        var fruitGrid = this.GetModel<IGameGridModel>().currentFruitGrid;
        fruitGrid.Clear();
        for (var rowIndex = 0; rowIndex < levelData.gridRow; ++rowIndex)
        {
            var row = new List<FruitCell>();
            for (var columIndex = 0; columIndex < levelData.gridColumn; ++columIndex)
            {
                var fruitType = levelData.GetRandomFruitType();
                var item = new FruitCell(fruitType, rowIndex, columIndex);
                row.Add(item);
            }
            // 存到列表中
            fruitGrid.Add(row);
        }
    }
    
    private List<FruitCell> DetectAllMatches()
    {
        var levelData = this.GetModel<ILevelModel>().currentLevelData;
        var fruitGrid = this.GetModel<IGameGridModel>().currentFruitGrid;
        var matches = new HashSet<FruitCell>();

        for (var row = 0; row < levelData.gridRow; row++)
        {
            var line = new List<FruitCell>();
            for (var col = 0; col < levelData.gridColumn; col++)
            {
                line.Add(fruitGrid[row][col]);
            }
            matches.UnionWith(DetectLineMatches(line));
        }

        for (var col = 0; col < levelData.gridColumn; col++)
        {
            var line = new List<FruitCell>();
            for (var row = 0; row < levelData.gridRow; row++)
            {
                line.Add(fruitGrid[row][col]);
            }
            matches.UnionWith(DetectLineMatches(line));
        }

        return new List<FruitCell>(matches);
    }
    
    private List<FruitCell> DetectLineMatches(List<FruitCell> line)
    {
        var matches = new List<FruitCell>();

        var i = 0;

        while (i < line.Count)
        {
            if (line[i] == null)
            {
                i++;
                continue;
            }

            var type = line[i].fruitType;
            var start = i;

            while (i < line.Count && line[i] != null && line[i].fruitType == type)
            {
                i++;
            }

            var count = i - start;
            if (count >= 3)
            {
                for (var j = start; j < i; j++)
                    matches.Add(line[j]);
            }
        }

        return matches;
    }
}
