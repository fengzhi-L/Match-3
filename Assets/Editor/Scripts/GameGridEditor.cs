// Assets/Editor/GameGridEditor.cs
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameGridEditor : EditorWindow
{
    private LevelData currentLevelData;
    private List<List<GridCell>> gridData;
    private Vector2 scrollPosition;
    private int selectedCellType = 0;
    private string levelName = "NewLevel";
    private string savePath = "Assets/Resources/Levels/";

    private int currentRows = 8;
    private int currentCols = 8;

    [MenuItem("Tools/Game Grid Editor")]
    public static void ShowWindow()
    {
        GetWindow<GameGridEditor>("棋盘编辑器");
    }

    private void OnEnable()
    {
        // 初始化网格数据
        if (gridData == null)
        {
            gridData = new List<List<GridCell>>();
            InitializeGrid(8, 8); // 默认8x8
        }
    }

    private void InitializeGrid(int rows, int cols)
    {
        gridData.Clear();
        for (int row = 0; row < rows; row++)
        {
            var rowData = new List<GridCell>();
            for (int col = 0; col < cols; col++)
            {
                rowData.Add(new GridCell(CellType.BaseBlock, row, col));
            }
            gridData.Add(rowData);
        }
        
        currentRows = rows;
        currentCols = cols;
    }

    private void OnGUI()
    {
        GUILayout.Label("棋盘编辑器", EditorStyles.boldLabel);

        // 关卡信息设置
        EditorGUILayout.BeginVertical("Box");
        levelName = EditorGUILayout.TextField("关卡名称:", levelName);
        EditorGUILayout.EndVertical();

        // 网格尺寸设置
        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label("网格设置", EditorStyles.boldLabel);
        int newRows = EditorGUILayout.IntField("行数:", gridData.Count);
        int newCols = 0;
        if (gridData.Count > 0)
        {
            newCols = EditorGUILayout.IntField("列数:", gridData[0].Count);
        }
        
        if (GUILayout.Button("更新网格大小"))
        {
            UpdateGridSize(newRows, newCols);
        }
        EditorGUILayout.EndVertical();

        // 单元格类型选择
        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label("单元格类型", EditorStyles.boldLabel);
        string[] cellTypeNames = System.Enum.GetNames(typeof(CellType));
        selectedCellType = EditorGUILayout.Popup("类型:", selectedCellType, cellTypeNames);
        EditorGUILayout.EndVertical();

        // 网格编辑区域
        if (gridData.Count > 0 && gridData[0].Count > 0)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(400));
            DrawGrid();
            EditorGUILayout.EndScrollView();
        }

        // 操作按钮
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("清空棋盘"))
        {
            ClearGrid();
        }
        
        if (GUILayout.Button("随机填充"))
        {
            RandomFillGrid();
        }
        
        if (GUILayout.Button("加载关卡"))
        {
            //LoadLevelData();
        }
        
        EditorGUILayout.EndHorizontal();

        // 保存功能
        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label("保存设置", EditorStyles.boldLabel);
        savePath = EditorGUILayout.TextField("保存路径:", savePath);
        
        if (GUILayout.Button("保存关卡数据"))
        {
            //SaveLevelData();
        }
        
        EditorGUILayout.EndVertical();
    }

    private void UpdateGridSize(int newRows, int newCols)
    {
        if (newRows <= 0 || newCols <= 0) return;

        // 调整行数
        while (gridData.Count < newRows)
        {
            var newRow = new List<GridCell>();
            for (int col = 0; col < newCols; col++)
            {
                newRow.Add(new GridCell(CellType.BaseBlock, gridData.Count, col));
            }
            gridData.Add(newRow);
        }

        while (gridData.Count > newRows)
        {
            gridData.RemoveAt(gridData.Count - 1);
        }

        // 调整列数
        for (int row = 0; row < gridData.Count; row++)
        {
            while (gridData[row].Count < newCols)
            {
                gridData[row].Add(new GridCell(CellType.BaseBlock, row, gridData[row].Count));
            }
            
            while (gridData[row].Count > newCols)
            {
                gridData[row].RemoveAt(gridData[row].Count - 1);
            }
            
            // 更新索引
            for (int col = 0; col < gridData[row].Count; col++)
            {
                gridData[row][col].rowIndex = row;
                gridData[row][col].colIndex = col;
            }
        }
        
        currentRows = newRows;
        currentCols = newCols;
    }

    private void DrawGrid()
    {
        for (int row = 0; row < gridData.Count; row++)
        {
            EditorGUILayout.BeginHorizontal();
            
            for (int col = 0; col < gridData[row].Count; col++)
            {
                GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
                buttonStyle.normal.background = GetTextureForCellType(gridData[row][col].cellType);
                
                if (GUILayout.Button($"", buttonStyle, GUILayout.Width(30), GUILayout.Height(30)))
                {
                    // 点击切换类型
                    CellType selectedType = (CellType)System.Enum.GetValues(typeof(CellType)).GetValue(selectedCellType);
                    gridData[row][col] = new GridCell(selectedType, row, col);
                }
                
                // 显示坐标（可选）
                // GUILayout.Label($"{row},{col}", GUILayout.Width(30), GUILayout.Height(30));
            }
            
            EditorGUILayout.EndHorizontal();
        }
    }

    private Texture2D GetTextureForCellType(CellType type)
    {
        Color color = GetColorForCellType(type);
        return MakeTex(30, 30, color);
    }

    private Color GetColorForCellType(CellType type)
    {
        switch (type)
        {
            case CellType.BaseBlock: return Color.gray;
            case CellType.Empty: return Color.white;
            // case CellType.Obstacle: return Color.black;
            // case CellType.Special: return Color.yellow;
            default: return Color.red;
        }
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    private void ClearGrid()
    {
        for (int row = 0; row < gridData.Count; row++)
        {
            for (int col = 0; col < gridData[row].Count; col++)
            {
                gridData[row][col] = new GridCell(CellType.BaseBlock, row, col);
            }
        }
    }

    private void RandomFillGrid()
    {
        for (int row = 0; row < gridData.Count; row++)
        {
            for (int col = 0; col < gridData[row].Count; col++)
            {
                CellType randomType = Random.value > 0.7f ? CellType.Empty : CellType.BaseBlock;
                gridData[row][col] = new GridCell(randomType, row, col);
            }
        }
    }

    // private void LoadLevelData()
    // {
    //     string path = EditorUtility.OpenFilePanel(
    //         "选择关卡文件", 
    //         "Assets/Resources/Levels/", 
    //         "asset"
    //     );
    //     
    //     if (!string.IsNullOrEmpty(path))
    //     {
    //         // 相对于Assets的路径
    //         string assetPath = "Assets" + path.Substring(Application.dataPath.Length);
    //         currentLevelData = AssetDatabase.LoadAssetAtPath<LevelData>(assetPath);
    //         
    //         if (currentLevelData != null)
    //         {
    //             gridData = new List<List<GridCellData>>();
    //             foreach (var row in currentLevelData.predefinedGrid)
    //             {
    //                 var newRow = new List<GridCellData>();
    //                 foreach (var cell in row)
    //                 {
    //                     newRow.Add(new GridCellData(cell.cellType, cell.rowIndex, cell.colIndex));
    //                 }
    //                 gridData.Add(newRow);
    //             }
    //             
    //             levelName = currentLevelData.levelName;
    //             Debug.Log($"成功加载关卡: {currentLevelData.levelName}");
    //         }
    //     }
    // }

    // private void SaveLevelData()
    // {
    //     if (string.IsNullOrEmpty(levelName))
    //     {
    //         EditorUtility.DisplayDialog("错误", "请输入关卡名称", "确定");
    //         return;
    //     }
    //
    //     // 创建LevelData资源
    //     LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
    //     levelData.levelName = levelName;
    //     levelData.gridRow = gridData.Count;
    //     levelData.gridColumn = gridData.Count > 0 ? gridData[0].Count : 0;
    //     
    //     // 复制网格数据
    //     levelData.predefinedGrid = new List<List<GridCellData>>();
    //     foreach (var row in gridData)
    //     {
    //         var newRow = new List<GridCellData>();
    //         foreach (var cell in row)
    //         {
    //             newRow.Add(new GridCellData(cell.cellType, cell.rowIndex, cell.colIndex));
    //         }
    //         levelData.predefinedGrid.Add(newRow);
    //     }
    //
    //     // 保存资源
    //     string fullPath = System.IO.Path.Combine(savePath, $"{levelName}.asset");
    //     string relativePath = "Assets" + fullPath.Substring(Application.dataPath.Length);
    //     
    //     System.IO.Directory.CreateDirectory(savePath);
    //     AssetDatabase.CreateAsset(levelData, relativePath);
    //     AssetDatabase.SaveAssets();
    //     AssetDatabase.Refresh();
    //     
    //     currentLevelData = levelData;
    //     Debug.Log($"关卡数据已保存: {relativePath}");
    //     EditorUtility.DisplayDialog("成功", $"关卡数据已保存至: {relativePath}", "确定");
    // }
}