using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridEditor : EditorWindow
{
    private EditorConfig editorConfig;
    private CellSpriteConfig cellSpriteConfig;
    private List<List<GridCell>> gridData;
    private Vector2 scrollPosition;
    private int selectedCellType = 0;
    private int gridRows = 8;
    private int gridCols = 8;
    private int tempRows = 8;
    private int tempCols = 8;
    
    [MenuItem("Tools/Grid Editor")]
    public static void ShowWindow()
    {
        GetWindow<GridEditor>("简单网格编辑器");
    }
    
    private void OnEnable()
    {
        InitializeGrid(gridRows, gridCols);
        LoadEditorConfig();
        tempRows = gridRows;
        tempCols = gridCols;
    }

    private void LoadEditorConfig()
    {
        string[] guids = AssetDatabase.FindAssets("t:GridEditorConfig");
        
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            editorConfig = AssetDatabase.LoadAssetAtPath<EditorConfig>(path);
            
            if (editorConfig != null)
            {
                cellSpriteConfig = editorConfig.cellSpriteConfig;
            }
        }
    }

    private void InitializeGrid(int rows, int cols)
    {
        gridData = new List<List<GridCell>>();
        for (int row = rows - 1; row >= 0; row--)
        {
            var rowData = new List<GridCell>();
            for (int col = cols - 1; col >= 0; col--)
            {
                rowData.Add(new GridCell(CellType.BaseBlock, row, col));
            }
            gridData.Add(rowData);
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("简单网格编辑器", EditorStyles.boldLabel);
        
        if (GUILayout.Button("序列化测试"))
        {
            TestJson();
        }
        
        // 网格尺寸设置
        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label("网格设置", EditorStyles.boldLabel);
        tempRows = EditorGUILayout.IntField("行数:", tempRows);
        tempCols = EditorGUILayout.IntField("列数:", tempCols);
        
        if (GUILayout.Button("更新网格大小"))
        {
            if (tempRows > 0 && tempCols > 0)
            {
                UpdateGridSize(tempRows, tempCols);
                gridRows = tempRows;
                gridCols = tempCols;
            }
            else
            {
                EditorUtility.DisplayDialog("错误", "行数和列数必须大于0", "确定");
            }
        }
        EditorGUILayout.EndVertical();
        
        // 当前尺寸显示
        GUILayout.Label($"当前尺寸: {gridRows} x {gridCols}", EditorStyles.miniLabel);
        
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
        
        EditorGUILayout.EndHorizontal();
        
        // 输出提示
        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label("使用说明:", EditorStyles.boldLabel);
        GUILayout.Label("1. 编辑完成后点击'生成网格资源'");
        GUILayout.Label("2. 在Project视图中找到生成的网格资源");
        GUILayout.Label("3. 拖拽到LevelData的predefinedGrid字段中");
        EditorGUILayout.EndVertical();

        if (GUILayout.Button("导出JSON到StreamingAssets"))
        {
            ExportAsJsonToStreamingAssets();
        }
    }

    private void DrawGrid()
    {
        // 定义颜色映射
        Dictionary<CellType, Color> colorMap = new Dictionary<CellType, Color>
        {
            { CellType.BaseBlock, Color.green },
            { CellType.Empty, Color.red }
        };
        
        GUIStyle cellStyle = new GUIStyle(GUI.skin.button);
        cellStyle.margin = new RectOffset(1, 1, 1, 1);
        
        for (int row = gridData.Count - 1; row >=0; row--)
        {
            EditorGUILayout.BeginHorizontal();

            for (int col = gridData[row].Count - 1; col >=0; col--)
            {
                CellType currentType = gridData[row][col].cellType;
                Color originalColor = GUI.color;
            
                // 设置按钮颜色
                GUI.color = colorMap.ContainsKey(currentType) ? colorMap[currentType] : Color.yellow;
                // GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
                // buttonStyle.normal.background = GetTextureForCellType(gridData[row][col].cellType);

                if (GUILayout.Button($"", cellStyle, GUILayout.Width(30), GUILayout.Height(30)))
                {
                    // 点击切换类型
                    CellType selectedType =
                        (CellType)System.Enum.GetValues(typeof(CellType)).GetValue(selectedCellType);
                    gridData[row][col] = new GridCell(selectedType, row, col);
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }
    
    private void UpdateGridSize(int newRows, int newCols)
    {
        if (newRows <= 0 || newCols <= 0) return;

        // 调整行数
        if (newRows > gridData.Count)
        {
            // 增加行
            for (int row = gridData.Count; row < newRows; row++)
            {
                var newRow = new List<GridCell>();
                for (int col = 0; col < newCols; col++)
                {
                    newRow.Add(new GridCell(CellType.BaseBlock, row, col));
                }
                gridData.Add(newRow);
            }
        }
        else if (newRows < gridData.Count)
        {
            // 减少行
            gridData.RemoveRange(newRows, gridData.Count - newRows);
        }

        // 调整列数
        for (int row = 0; row < gridData.Count; row++)
        {
            if (newCols > gridData[row].Count)
            {
                // 增加列
                for (int col = gridData[row].Count; col < newCols; col++)
                {
                    gridData[row].Add(new GridCell(CellType.BaseBlock, row, col));
                }
            }
            else if (newCols < gridData[row].Count)
            {
                // 减少列
                gridData[row].RemoveRange(newCols, gridData[row].Count - newCols);
            }
            
            // 更新索引
            for (int col = 0; col < gridData[row].Count; col++)
            {
                gridData[row][col].rowIndex = row;
                gridData[row][col].colIndex = col;
            }
        }
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
    
    private void GenerateGridResource()
    {
        // 创建一个临时的ScriptableObject来存储网格数据
        var gridContainer = CreateInstance<GridData>();
        gridContainer.Grid = gridData;
        
        // 保存资源
        string assetPath = EditorUtility.SaveFilePanelInProject(
            "保存网格资源",
            "GridData.asset",
            "asset",
            "保存编辑好的网格数据"
        );
        
        if (!string.IsNullOrEmpty(assetPath))
        {
            AssetDatabase.CreateAsset(gridContainer, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"网格资源已保存到: {assetPath}");
            
            // 选中刚创建的资源
            var createdAsset = AssetDatabase.LoadAssetAtPath<GridData>(assetPath);
            Selection.activeObject = createdAsset;
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
            case CellType.BaseBlock: return Color.green;
            case CellType.Empty: return Color.red;
            default: return Color.yellow;
        }
    }
    
    // private Texture2D MakeTex(int width, int height, Color col)
    // {
    //     Color[] pix = new Color[width * height];
    //     for (int i = 0; i < pix.Length; i++)
    //     {
    //         pix[i] = col;
    //     }
    //     Texture2D result = new Texture2D(width, height);
    //     result.SetPixels(pix);
    //     result.Apply();
    //     return result;
    // }
    
    private Texture2D MakeTex(int width, int height, Color col)
    {
        Texture2D result = new Texture2D(width, height);
    
        // 创建像素数组
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = col;
        }
    
        result.SetPixels(pix);
        result.Apply();
    
        // 设置纹理参数以提高兼容性
        result.wrapMode = TextureWrapMode.Clamp;
        result.filterMode = FilterMode.Point;
        result.hideFlags = HideFlags.HideAndDontSave;
    
        return result;
    }
    
    private void ExportAsJsonToStreamingAssets()
    {
        string fileName = EditorUtility.SaveFilePanel(
            "导出网格数据",
            Application.streamingAssetsPath + "/GridData/",
            "GridData.json",
            "json"
        );
    
        if (!string.IsNullOrEmpty(fileName))
        {
            // 提取文件名（不含路径）
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        
            // 确保目录存在
            string directory = Path.GetDirectoryName(fileName);
            Directory.CreateDirectory(directory);
        
            // 保存数据
            var json = JsonConvert.SerializeObject(gridData);
            
            string directory1= Application.streamingAssetsPath + "/GridData/";
            Directory.CreateDirectory(directory1);
        
            string filePath = Path.Combine(directory1, $"{fileName}");
            File.WriteAllText(filePath, json);
            Debug.Log($"网格数据已保存到: {filePath}");
        
            AssetDatabase.Refresh();
            Debug.Log($"网格数据已导出到: {fileName}");
        }
    }

    private void TestJson()
    {
        var a = JsonConvert.SerializeObject(gridData);
        Debug.Log(a);
    }

}

