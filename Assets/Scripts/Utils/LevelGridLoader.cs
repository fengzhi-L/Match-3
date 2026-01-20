using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LevelGridLoader
{
    [Serializable]
    public class SerializableGridCell
    {
        public int cellType; // 使用整数存储CellType
        public int rowIndex;
        public int colIndex;
    }
    
    [Serializable]
    public class SerializableGridData
    {
        public List<List<SerializableGridCell>> gridData = new List<List<SerializableGridCell>>();
        public int rows;
        public int cols;
    }
    
    public static List<List<GridCell>> LoadGridFromJson(string levelName)
    {
        // 尝试从Resources加载JSON文件
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/Levels/Grids/{levelName}_Grid");
        
        if (textAsset != null)
        {
            try
            {
                SerializableGridData data = JsonUtility.FromJson<SerializableGridData>(textAsset.text);
                return ConvertToGridCells(data);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"加载网格数据失败: {e.Message}");
                return GenerateEmptyGrid(8, 8); // 返回默认网格
            }
        }

        return null;
    }
    
    private static List<List<GridCell>> ConvertToGridCells(SerializableGridData data)
    {
        var result = new List<List<GridCell>>();
        foreach (var row in data.gridData)
        {
            var gridRow = new List<GridCell>();
            foreach (var cell in row)
            {
                var cellType = (CellType)cell.cellType;
                gridRow.Add(new GridCell(cellType, cell.rowIndex, cell.colIndex));
            }
            result.Add(gridRow);
        }
        return result;
    }
    
    private static List<List<GridCell>> GenerateEmptyGrid(int rows, int cols)
    {
        var grid = new List<List<GridCell>>();
        for (int row = 0; row < rows; row++)
        {
            var rowList = new List<GridCell>();
            for (int col = 0; col < cols; col++)
            {
                rowList.Add(new GridCell(CellType.BaseBlock, row, col));
            }
            grid.Add(rowList);
        }
        return grid;
    }
    
    public static void SaveGridToJson(List<List<GridCell>> grid, int rows, int cols, string fileName)
    {
        var serializableData = new SerializableGridData();
        serializableData.rows = rows;
        serializableData.cols = cols;

        foreach (var gridRow in grid)
        {
            var serializableRow = new List<SerializableGridCell>();
            foreach (var cell in gridRow)
            {
                var serializableCell = new SerializableGridCell
                {
                    cellType = (int)cell.cellType,
                    rowIndex = cell.rowIndex,
                    colIndex = cell.colIndex
                };
                serializableRow.Add(serializableCell);
            }
            serializableData.gridData.Add(serializableRow);
        }

        string json = JsonUtility.ToJson(serializableData, true);
        
        // 确保目录存在
        string directory = Application.streamingAssetsPath + "/GridData/";
        Directory.CreateDirectory(directory);
        
        string filePath = Path.Combine(directory, $"{fileName}_Grid.json");
        File.WriteAllText(filePath, json);
        Debug.Log($"网格数据已保存到: {filePath}");
    }
}
