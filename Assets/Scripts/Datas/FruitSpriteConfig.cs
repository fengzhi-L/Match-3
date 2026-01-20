using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FruitSpriteConfig", menuName = "Game/Fruit Sprite Config")]
public class FruitSpriteConfig : ScriptableObject
{
    [Serializable]
    public class FruitSpriteItem
    {
        public FruitType fruitType;
        public Sprite sprite;
    }
    
    public List<FruitSpriteItem> fruitSprites = new ();
    private Dictionary<FruitType, Sprite> _spriteMap;
    
    public Dictionary<FruitType, Sprite> GetPrefabMap()
    {
        if (_spriteMap == null)
        {
            _spriteMap = new Dictionary<FruitType, Sprite>();
            foreach (var item in fruitSprites)
            {
                if (item.sprite != null)
                    _spriteMap[item.fruitType] = item.sprite;
            }
        }

        return _spriteMap;
    }
    
    public Sprite GetSprite(FruitType type)
    {
        return GetPrefabMap().GetValueOrDefault(type);
    }
}
