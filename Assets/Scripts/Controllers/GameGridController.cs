using QFramework;
using UnityEngine;

public class GameGridController : MonoBehaviour, IController
{
    public GameObject cellPrefab;
    public Transform gridParent;
    
    void Start()
    {
        if (cellPrefab == null)
        {
            Debug.LogError("未分配 cellPrefab!");
            return;
        }

        if (gridParent == null)
        {
            gridParent = transform; // 默认自身为父级
        }

        SpawnGrid();
    }

    void SpawnGrid()
    {
        var levelData = this.GetModel<ILevelModel>().GetCurrentLevelData();
        var halfWidth = (levelData.gridWidth - 1) / 2f;
        var halfHeight = (levelData.gridHeight - 1) / 2f;

        for (var row = 0; row < levelData.gridHeight; row++)
        {
            for (var col = 0; col < levelData.gridWidth; col++)
            {
                var cell = Instantiate(cellPrefab, gridParent);

                var x = col - halfWidth;
                var y = row - halfHeight;

                cell.transform.localPosition = new Vector3(x, y, 0);
            }
        }
    }
    
    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}