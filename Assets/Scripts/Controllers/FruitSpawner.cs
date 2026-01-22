using System.Collections.Generic;
using QFramework;
using UnityEngine;

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
        RefreshGridView();

        this.RegisterEvent<GenerateFruitEvent>(e => { GenerateFruitItem(e.Row, e.Column); }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void RefreshGridView()
    {
        var levelData = this.GetModel<ILevelModel>().currentLevelData;
        var fruitGrid = this.GetModel<IGameGridModel>().currentFruitGrid;
        var fruitItemGrid = this.GetModel<IFruitModel>().fruitGrid;
        fruitItemGrid.Clear();
        
        var halfWidth = (levelData.gridColumn - 1) / 2f;
        var halfHeight = (levelData.gridRow - 1) / 2f;

        foreach (var rowCells in fruitGrid)
        {
            var row = new List<FruitItem>();
            foreach (var fruitCell in rowCells)
            {
                if (fruitCell == null)
                {
                    row.Add(null);
                }
                else
                {
                    var prefab = prefabConfig.GetPrefab(fruitCell.fruitType);
                    if(prefab == null) continue;

                    var cellItem = Instantiate(fruitItem, _fruitRoot, false);
                    var bhv = cellItem.GetComponent<FruitItem>();
                    var x = fruitCell.colIndex - halfWidth;
                    var y = fruitCell.rowIndex - halfHeight;
                    var targetPos = new Vector3(x, y, 0);
                    bhv.Initialize(prefabConfig.GetPrefabMap());
                    bhv.SetFruitType(fruitCell.fruitType);
                    bhv.SetPosition(fruitCell.rowIndex, fruitCell.colIndex, targetPos);

                    cellItem.transform.localPosition = new Vector3(x, y, 0);
                    row.Add(bhv);
                }
            }
            fruitItemGrid.Add(row);
        }
    }
    
    private void GenerateFruitItem(int row, int column)
    {
        var levelData = this.GetModel<ILevelModel>().currentLevelData;
        var halfWidth = (levelData.gridColumn - 1) / 2f;
        var halfHeight = (levelData.gridRow - 1) / 2f;
        
        var cellItem = Instantiate(fruitItem, _fruitRoot, false);
        var bhv = cellItem.GetComponent<FruitItem>();
        var x = column - halfWidth;
        var y = row - halfHeight;
        bhv.transform.localPosition = new Vector3(x, y + 2, 0);
        var targetPos = new Vector3(x, y, 0);
        bhv.Initialize(prefabConfig.GetPrefabMap());
        bhv.SetFruitType(levelData.GetRandomFruitType());
        bhv.SetPosition(row, column, targetPos, true);

        this.GetModel<IFruitModel>().newFruit = bhv;
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}