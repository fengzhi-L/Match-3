using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class GameGridSystem : AbstractSystem, IGameGridSystem
{
    private GridCell[,] _grid;
    private LevelData _levelData;
    private System.Random _random;

    public int width => _levelData.gridWidth;
    public int height => _levelData.gridHeight;
    
    protected override void OnInit()
    {
        _random = new System.Random();
    }
    
}
