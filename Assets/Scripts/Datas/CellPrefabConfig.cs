using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CellPrefabConfig", menuName = "Game/Cell Prefab Config")]
public class CellPrefabConfig : ScriptableObject
{
    [Serializable]
    public class CellPrefabItem
    {
        public CellType cellType;
        public GameObject prefab;
    }
    
    public List<CellPrefabItem> cellPrefabs = new ();
    private Dictionary<CellType, GameObject> _prefabMap;

    public Dictionary<CellType, GameObject> GetPrefabMap()
    {
        if (_prefabMap == null)
        {
            _prefabMap = new Dictionary<CellType, GameObject>();
            foreach (var item in cellPrefabs)
            {
                if (item.prefab != null)
                    _prefabMap[item.cellType] = item.prefab;
            }
        }

        return _prefabMap;
    }

    public GameObject GetPrefab(CellType type)
    {
        return GetPrefabMap().GetValueOrDefault(type);
    }
}