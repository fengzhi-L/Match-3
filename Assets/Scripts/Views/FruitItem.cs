using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FruitItem : MonoBehaviour
{
    public int rowIndex;
    public int columnIndex;
    public FruitType fruitType;
    public GameObject fruitPrefab;
    private Transform _selfTransform;
    private Dictionary<FruitType, GameObject> _prefabs;

    private void Awake()
    {
        _selfTransform = transform;
    }

    public void Initialize(Dictionary<FruitType, GameObject> prefabs)
    {
        _prefabs = prefabs;
    }

    public void SetFruitType(FruitType type)
    {
        fruitType = type;
        if (_prefabs != null && _prefabs.TryGetValue(type, out var prefab))
        {
            fruitPrefab = _prefabs.GetValueOrDefault(type);
            Instantiate(fruitPrefab, _selfTransform, false);
            gameObject.name = $"Fruit_{type}";
        }
        else
        {
            Debug.LogWarning($"[FruitItem] 没有找到{type}的预制体");
        }
    }

    public void SetPosition(int row, int column, Vector3 position, bool dotween = false)
    {
        rowIndex = row;
        columnIndex = column;
        if (dotween)
        {
            _selfTransform.DOLocalMove(position, 0.3f);
        }
        else
        {
            _selfTransform.localPosition = position;
        }
    }
}
