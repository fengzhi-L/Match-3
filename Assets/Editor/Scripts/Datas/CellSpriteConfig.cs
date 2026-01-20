using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CellSpriteConfig", menuName = "Game/Cell Sprite Config")]
public class CellSpriteConfig : ScriptableObject
{
    [Serializable]
    public class CellSpriteItem
    {
        public CellType cellType;
        public Sprite sprite;
    }
    
    public List<CellSpriteItem> cellSprites = new ();
    private Dictionary<CellType, Sprite> _spriteMap;
    
    public Dictionary<CellType, Sprite> GetPrefabMap()
    {
        if (_spriteMap == null)
        {
            _spriteMap = new Dictionary<CellType, Sprite>();
            foreach (var item in cellSprites)
            {
                if (item.sprite != null)
                    _spriteMap[item.cellType] = item.sprite;
            }
        }

        return _spriteMap;
    }
    
    public Sprite GetSprite(CellType type)
    {
        return GetPrefabMap().GetValueOrDefault(type);
    }
}
