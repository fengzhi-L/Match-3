using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class FruitSpawner : MonoBehaviour, IController
{
    public List<GameObject> fruitPrefabs;
    
    private List<List<FruitItem>> _fruitGrid = new();
    
    private Transform _fruitRoot;

    private void Awake()
    {
        _fruitRoot = transform;
    }

    private void Start()
    {
        SpawnFruitGrid();
    }

    /// <summary>
    /// 生成多行多列水果
    /// </summary>
    private void SpawnFruitGrid()
    {
        var levelData = this.GetModel<ILevelModel>().GetCurrentLevelData();
        _fruitGrid.Clear();
        for (var rowIndex = 0; rowIndex < levelData.gridHeight; ++rowIndex)
        {
            var row = new List<FruitItem>();
            for (var columIndex = 0; columIndex < levelData.gridWidth; ++columIndex)
            {
                var item = AddRandomFruitItem(rowIndex, columIndex);
                row.Add(item);
            }
            // 存到列表中
            _fruitGrid.Add(row);
        }
    }

    /// <summary>
    /// 随机一个水果
    /// </summary>
    private FruitItem AddRandomFruitItem(int rowIndex, int columIndex)
    {
        var levelData = this.GetModel<ILevelModel>().GetCurrentLevelData();
        // 随机一个水果类型
        var fruitType = Random.Range(0, levelData.availableFruits.Length);
        var item = new GameObject("FruitItem");
        item.transform.SetParent(_fruitRoot, false);
        item.AddComponent<BoxCollider2D>().size = Vector2.one;
        var bhv = item.AddComponent<FruitItem>();
        bhv.UpdatePosition(rowIndex, columIndex, levelData);
        bhv.CreateFruitBg(fruitType, fruitPrefabs[fruitType]);
        return bhv;
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}