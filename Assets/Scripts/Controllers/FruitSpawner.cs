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
    }

    private void RefreshGridView()
    {
        var levelData = this.GetModel<ILevelModel>().currentLevelData;
        var grid = this.GetModel<IGameGridModel>().currentFruitGrid;
        var fruitGrid = this.GetModel<IFruitModel>().fruitGrid;
        fruitGrid.Clear();
        
        var halfWidth = (levelData.gridColumn - 1) / 2f;
        var halfHeight = (levelData.gridRow - 1) / 2f;

        foreach (var rowCells in grid)
        {
            var row = new List<FruitItem>();
            foreach (var fruitCell in rowCells)
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
            fruitGrid.Add(row);
        }
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}