using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FruitPrefabConfig", menuName = "Game/Fruit Prefab Config")]
public class FruitPrefabConfig : ScriptableObject
{
    [Serializable]
    public class FruitPrefabItem
    {
        public FruitType fruitType;
        public GameObject prefab;
    }
    
    public List<FruitPrefabItem> fruitPrefabs = new ();
    private Dictionary<FruitType, GameObject> _prefabMap;

    public Dictionary<FruitType, GameObject> GetPrefabMap()
    {
        if (_prefabMap == null)
        {
            _prefabMap = new Dictionary<FruitType, GameObject>();
            foreach (var item in fruitPrefabs)
            {
                if (item.prefab != null)
                    _prefabMap[item.fruitType] = item.prefab;
            }
        }

        return _prefabMap;
    }

    public GameObject GetPrefab(FruitType type)
    {
        return GetPrefabMap().GetValueOrDefault(type);
    }
}
