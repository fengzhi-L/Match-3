using System;
using QFramework;
using UnityEngine;

public class GameGridController : MonoBehaviour, IController
{
    [Header("配置")] public GameObject gridCellPrefab;
    public Transform container;
    public Vector2 cellSize = new (1f, 1f);
    public Vector2 spacing = Vector2.zero;
    
    private GridCell[,] _currentGrid;

    private void Awake()
    {
        
    }

    private void Start()
    {
        Initialize(8, 8);
    }

    public void Initialize(int width, int height)
    {
        var gameGridModel = this.GetModel<IGameGridModel>();
        if (container == null)
        {
            container = new GameObject("GameGrid").transform;
            container.SetParent(transform, false);
        }

        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
        
        gameGridModel.SetCurrentGrid(width, height);
        _currentGrid = gameGridModel.GetCurrentGrid();
        
        var startX = -(width - 1) * (cellSize.x + spacing.x) * 0.5f;
        var startY = (height - 1) * (cellSize.y + spacing.y) * 0.5f;
        
        for (var row = 0; row < height; row++)
        {
            for (var col = 0; col < width; col++)
            {
                var cellObj = Instantiate(gridCellPrefab, container);
                cellObj.name = $"Cell_BG_{row}_{col}";
                
                var x = startX + col * (cellSize.x + spacing.x);
                var y = startY - row * (cellSize.y + spacing.y);
                cellObj.transform.localPosition = new Vector3(x, y, 0);
                cellObj.transform.localScale = new Vector3(cellSize.x, cellSize.y, 1);
                
                _currentGrid[row, col] = new GridCell(cellObj, row, col);
            }
        }
    }

    public void HighlightCell(int row, int col, bool isHighlight)
    {
        if (_currentGrid != null &&
            row >= 0 && row < _currentGrid.GetLength(0) &&
            col >= 0 && col < _currentGrid.GetLength(1))
        {
            var spriteRenderer = _currentGrid[row, col].cellBg?.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = isHighlight ? Color.yellow : Color.white;
            }
        }
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
