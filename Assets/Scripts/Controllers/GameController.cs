using System.Collections;
using QFramework;
using UnityEngine;

public class GameController : MonoBehaviour, IController
{
    private void Start()
    {
        this.RegisterEvent<FruitSelectedEvent>(e => { OnFruitSelected(e.FruitItem); });
        this.RegisterEvent<FruitUnSelectedEvent>(e => { OnFruitUnSelect(e.FruitItem); });
        this.RegisterEvent<FruitMoveEvent>(e => { OnFruitMove(e.Delta); });
    }
    
    private void OnFruitSelected(FruitItem item)
    {
        Debug.Log("改变当前水果");
        this.GetModel<IFruitModel>().currentSelectedFruit = item;
    }

    private void OnFruitUnSelect(FruitItem item)
    {
        this.GetModel<IFruitModel>().currentSelectedFruit = null;
    }

    private void OnFruitMove(Vector3 delta)
    {
        var currentSelectedFruit = this.GetModel<IFruitModel>().currentSelectedFruit;
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            // 水平滑动
            var direction = delta.x > 0 ? 1 : -1;
            var target = GetFruitItem(currentSelectedFruit.rowIndex, currentSelectedFruit.columnIndex + direction);
            if (target != null) StartCoroutine(ExchangeAndMatch(currentSelectedFruit, target));
        }
        else
        {
            // 垂直滑动
            int direction = delta.y > 0 ? 1 : -1;
            var target = GetFruitItem(currentSelectedFruit.rowIndex + direction, currentSelectedFruit.columnIndex);
            if (target != null) StartCoroutine(ExchangeAndMatch(currentSelectedFruit, target));
        }
    }

    IEnumerator ExchangeAndMatch(FruitItem a, FruitItem b)
    {
        if(a == null || b == null) yield break;
        if(a == b) yield break;
        Exchange(a, b);
    }
    
    private void Exchange(FruitItem a, FruitItem b)
    {
        SetFruitItem(a.rowIndex, a.columnIndex, b);
        SetFruitItem(b.rowIndex, b.columnIndex, a);
        
        (a.rowIndex, b.rowIndex) = (b.rowIndex, a.rowIndex);
        (a.columnIndex, b.columnIndex) = (b.columnIndex, a.columnIndex);
        
        a.SetPosition(a.rowIndex, a.columnIndex, GetTargetPosition(a.rowIndex, a.columnIndex));
        b.SetPosition(b.rowIndex, b.columnIndex, GetTargetPosition(b.rowIndex, b.columnIndex));

        this.GetModel<IFruitModel>().currentSelectedFruit = null;
    }

    private Vector3 GetTargetPosition(int row, int col)
    {
        var levelData = this.GetModel<ILevelModel>().currentLevelData;
        var halfWidth = (levelData.gridColumn - 1) / 2f;
        var halfHeight = (levelData.gridRow - 1) / 2f;
        var x = col - halfWidth;
        var y = row - halfHeight;
        var targetPos = new Vector3(x, y, 0);
        return targetPos;
    }
    
    private FruitItem GetFruitItem(int row, int column)
    {
        if (row < 0 || row >= this.GetModel<IFruitModel>().fruitGrid.Count) 
            return null;
    
        if (column < 0 || column >= this.GetModel<IFruitModel>().fruitGrid[row].Count) 
            return null;
        return this.GetModel<IFruitModel>().fruitGrid[row][column];
    }
    
    private void SetFruitItem(int row, int column, FruitItem item)
    {
        this.GetModel<IFruitModel>().fruitGrid[row][column] = item;
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
