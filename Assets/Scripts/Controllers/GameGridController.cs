using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.Pool;

public class GameGridController : MonoBehaviour, IController
{
    [SerializeField] private CellPrefabConfig cellPrefabConfig;
    public Transform gridParent;
    public GameObject cellEntry;
    
    private List<GameObject> _pooledObjects = new();
    private ObjectPool<GameObject> _cellPool;
    private Dictionary<(int, int), GameObject> _cellEntrys = new();
    
    void Start()
    {
        _cellPool = new ObjectPool<GameObject>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            false,
            100,
            200
        );
        if (gridParent == null)
        {
            gridParent = transform; // 默认自身为父级
        }

        this.RegisterEvent<CellPrefabChangedEvent>(e => { RefreshGridView(); });
        
        RefreshGridView();
    }

    private GameObject CreatePooledItem()
    {
        var obj = Instantiate(cellEntry, gridParent);
        obj.SetActive(false);
        _pooledObjects.Add(obj);
        return obj;
    }

    private void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
        // 可以重置对象状态（如位置颜色动画）
        var bhv = obj.GetComponent<CellEntry>();
        if (bhv != null)
        {
            // 重置逻辑
        }
    }

    private void OnDestroyPoolObject(GameObject obj)
    {
        _pooledObjects.Remove(obj);
        Destroy(obj);
    }

    private void RefreshGridView()
    {
        Debug.Log("RefreshGridView");
        var levelData = this.GetModel<ILevelModel>().currentLevelData;
        var grid = this.GetModel<IGameGridModel>().currentGrid;
        
        var halfWidth = (levelData.gridColumn - 1) / 2f;
        var halfHeight = (levelData.gridRow - 1) / 2f;

        foreach (var rowCells in grid)
        {
            foreach (var cell in rowCells)
            {
                var prefab = cellPrefabConfig.GetPrefab(cell.cellType);
                if(prefab == null) continue;

                var cellItem = Instantiate(cellEntry, gridParent, false);
                var bhv = cellItem.GetComponent<CellEntry>();
                var x = cell.colIndex - halfWidth;
                var y = cell.rowIndex - halfHeight;
                var targetPos = new Vector3(x, y, 0);
                bhv.Initialize(cellPrefabConfig.GetPrefabMap());
                bhv.SetCellType(cell.cellType);
                bhv.SetPosition(cell.rowIndex, cell.colIndex, targetPos);

                cellItem.transform.localPosition = new Vector3(x, y, 0);
            }
        }
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}