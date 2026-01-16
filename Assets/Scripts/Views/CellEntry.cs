using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellEntry : MonoBehaviour
{
    public int rowIndex;
    public int columIndex;
    public CellType cellType;
    public GameObject cellPrefab;
    private Transform _selfTransform;
    private Dictionary<CellType, GameObject> _prefabs;

    private void Awake()
    {
        _selfTransform = transform;
    }

    public void Initialize(Dictionary<CellType, GameObject> prefabs)
    {
        _prefabs = prefabs;
    }

    public void SetCellType(CellType type)
    {
        cellType = type;
        if (_prefabs != null && _prefabs.TryGetValue(type, out var prefab))
        {
            cellPrefab = _prefabs.GetValueOrDefault(type);
            Instantiate(cellPrefab, _selfTransform, false);
            gameObject.name = $"Cell_{type}";
        }
        else
        {
            Debug.LogWarning($"[CellEntry] 没有找到{type}的预制体");
        }
    }

    public void SetPosition(int row, int column, Vector3 position)
    {
        rowIndex = row;
        columIndex = column;
        _selfTransform.localPosition = position;
    }
}
