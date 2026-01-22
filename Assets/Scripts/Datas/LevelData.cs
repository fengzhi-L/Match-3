using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Level_01", menuName = "Game/Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    [Header("基本信息")] 
    public int levelNumber = 1;
    public string levelName = "Level_01";

    [Header("棋盘设置")] 
    [Range(4, 12)] 
    public int gridColumn = 8;
    [Range(4, 12)] 
    public int gridRow = 8;
    
    public List<List<GridCell>> PredefinedGrid = new();

    [Header("游戏目标")] 
    public int targetScore = 1000;
    [Range(5, 50)] public int movesLimit = 30;
    public int score1Star;
    public int score2Star;
    public int score3Star;

    [Header("可用水果类型")] 
    public FruitType[] availableFruits = new FruitType[]
    {
        FruitType.Apple,
        FruitType.Banana,
        FruitType.Grape,
        FruitType.Orange,
        FruitType.Strawberry,
    };
    
    [Header("高级目标（可选）")]
    [Tooltip("需要消除的特定水果数量（按索引对应 availableFruits[]）")]
    public int[] targetFruitCounts = Array.Empty<int>(); // 长度应与 availableFruits 一致或为空

    [Header("障碍物/特殊机制（预留）")] 
    [Tooltip("未来可扩展：冰块位置、传送门等")]
    public bool hasObstacles = false;

    // 检查目标是否达成
    public bool IsTargetMet(int currentScore, int[] eliminatedFruitCounts = null)
    {
        if (currentScore < targetScore) return false;

        // 高级目标：消除特定水果数量
        if (targetFruitCounts is { Length: > 0 } targets)
        {
            if (eliminatedFruitCounts == null || eliminatedFruitCounts.Length != targets.Length) return false;

            for (var i = 0; i < targets.Length; i++)
            {
                if (eliminatedFruitCounts[i] < targets[i]) return false;
            }
        }

        return true;
    }

    public FruitType GetRandomFruitType()
    {
        var randomIndex = Random.Range(0, availableFruits.Length);
        return availableFruits[randomIndex];
    }
}
