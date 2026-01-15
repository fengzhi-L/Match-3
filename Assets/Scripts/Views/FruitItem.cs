using UnityEngine;

public class FruitItem : MonoBehaviour
{

    public int rowIndex;
    public int columIndex;
    public int fruitType;
    public GameObject fruitSprite;
    private Transform _selfTransform;

    private void Awake()
    {
        _selfTransform = transform;
    }
    
    public void CreateFruitBg(int type, GameObject prefab)
    {
        if (null != fruitSprite) return;
        fruitType = type;
        fruitSprite = Instantiate(prefab, _selfTransform, false);
    }
    
    public void UpdatePosition(int row, int column, LevelData levelData)
    {
        rowIndex = row;
        columIndex = column;
        var halfWidth = (levelData.gridWidth - 1) / 2f;
        var halfHeight = (levelData.gridHeight - 1) / 2f;
        var x = column - halfWidth;
        var y = row - halfHeight;
        var targetPos = new Vector3(x, y, 0);
        _selfTransform.localPosition = targetPos;
    }
}
