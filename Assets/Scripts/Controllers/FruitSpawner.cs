using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitSpawner : MonoBehaviour, IController
{
    [SerializeField] private FruitPrefabConfig prefabConfig;
    public GameObject fruitItem;
    
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
        var levelData = this.GetModel<ILevelModel>().currentLevelData;
        var fruitGrid = this.GetModel<IGameGridModel>().currentFruitGrid;
        fruitGrid.Clear();
        for (var rowIndex = 0; rowIndex < levelData.gridRow; ++rowIndex)
        {
            var row = new List<FruitItem>();
            for (var columIndex = 0; columIndex < levelData.gridColumn; ++columIndex)
            {
                var item = AddRandomFruitItem(rowIndex, columIndex);
                row.Add(item);
            }
            // 存到列表中
            fruitGrid.Add(row);
        }
    }

    /// <summary>
    /// 随机一个水果
    /// </summary>
    private FruitItem AddRandomFruitItem(int rowIndex, int columIndex)
    {
        var levelData = this.GetModel<ILevelModel>().currentLevelData;
        // 随机一个水果类型
        var fruitType = Random.Range(0, levelData.availableFruits.Length);
        var item = Instantiate(fruitItem, _fruitRoot, false);
        item.AddComponent<BoxCollider2D>().size = Vector2.one;
        var bhv = item.GetComponent<FruitItem>();
        var halfWidth = (levelData.gridColumn - 1) / 2f;
        var halfHeight = (levelData.gridRow - 1) / 2f;
        var x = columIndex - halfWidth;
        var y = rowIndex - halfHeight;
        var targetPos = new Vector3(x, y, 0);
        bhv.Initialize(prefabConfig.GetPrefabMap());
        bhv.SetFruitType(levelData.availableFruits[fruitType]);
        bhv.SetPosition(rowIndex, columIndex, targetPos);
        return bhv;
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}