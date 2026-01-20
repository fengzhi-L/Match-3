using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class GameController : MonoBehaviour, IController
{
    private List<List<FruitItem>> _fruitGrid;
    private LevelData _currentLevel;

    private void Awake()
    {
        this.SendCommand<LoadLevelCommand>();
    }

    private void Start()
    {
        _fruitGrid = this.GetModel<IFruitModel>().fruitGrid;
        _currentLevel = this.GetModel<ILevelModel>().currentLevelData;
        
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
        yield return new WaitForSeconds(0.3f);

        var matches = DetectAllMatches();

        if (matches.Count == 0)
        {
            Exchange(a, b);
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            this.SendCommand<FruitMoveSuccessCommand>();
            yield return StartCoroutine(ProcessCrush());
        }

        yield return null;
    }
    
    private void Exchange(FruitItem a, FruitItem b)
    {
        SetFruitItem(a.rowIndex, a.columnIndex, b);
        SetFruitItem(b.rowIndex, b.columnIndex, a);
        
        (a.rowIndex, b.rowIndex) = (b.rowIndex, a.rowIndex);
        (a.columnIndex, b.columnIndex) = (b.columnIndex, a.columnIndex);
        
        a.SetPosition(a.rowIndex, a.columnIndex, GetTargetPosition(a.rowIndex, a.columnIndex), true);
        b.SetPosition(b.rowIndex, b.columnIndex, GetTargetPosition(b.rowIndex, b.columnIndex), true);

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

    private List<FruitItem> DetectAllMatches()
    {
        var levelData = this.GetModel<ILevelModel>().currentLevelData;
        var fruitGrid = this.GetModel<IFruitModel>().fruitGrid;
        var matches = new HashSet<FruitItem>();

        for (var row = 0; row < levelData.gridRow; row++)
        {
            var line = new List<FruitItem>();
            for (var col = 0; col < levelData.gridColumn; col++)
            {
                line.Add(fruitGrid[row][col]);
            }
            matches.UnionWith(DetectLineMatches(line));
        }

        for (var col = 0; col < levelData.gridColumn; col++)
        {
            var line = new List<FruitItem>();
            for (var row = 0; row < levelData.gridRow; row++)
            {
                line.Add(fruitGrid[row][col]);
            }
            matches.UnionWith(DetectLineMatches(line));
        }

        return new List<FruitItem>(matches);
    }
    
    private List<FruitItem> DetectLineMatches(List<FruitItem> line)
    {
        var matches = new List<FruitItem>();

        var i = 0;

        while (i < line.Count)
        {
            if (line[i] == null)
            {
                i++;
                continue;
            }

            var type = line[i].fruitType;
            var start = i;

            while (i < line.Count && line[i] != null && line[i].fruitType == type)
            {
                i++;
            }

            var count = i - start;
            if (count >= 3)
            {
                for (var j = start; j < i; j++)
                    matches.Add(line[j]);
            }
        }

        return matches;
    }

    private IEnumerator ProcessCrush()
    {
        var hasMatches = true;
        while (hasMatches)
        {
            var matches = DetectAllMatches();
            if (matches.Count == 0)
            {
                hasMatches = false;
                break;
            }

            foreach (var item in matches)
            {
                //todo: 粒子特效，音效等
                Destroy(item.gameObject);
                _fruitGrid[item.rowIndex][item.columnIndex] = null;
                
                this.SendCommand(new FruitCrushCommand(item.transform.localPosition));
                this.SendCommand(new GetScoreCommand(item.transform.localPosition, 10)); // 写死10分
            }

            FallDown();

            GenerateNewFruits();
            
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void FallDown()
    {
        for (var col = 0; col < _currentLevel.gridColumn; col++)
        {
            for (var row = 0; row < _currentLevel.gridRow; row++)
            {
                if (_fruitGrid[row][col] == null)
                {
                    // 找到了空位，现在从上方（更高 row）找最近的水果
                    int sourceRow = row + 1;
                    for (; sourceRow < _currentLevel.gridRow; sourceRow++)
                    {
                        if (_fruitGrid[sourceRow][col] != null)
                        {
                            // 找到水果，让它“掉下来”到空位
                            var item = _fruitGrid[sourceRow][col];
                            _fruitGrid[row][col] = item;
                            _fruitGrid[sourceRow][col] = null;

                            // 更新水果的行列信息
                            item.rowIndex = row;
                            item.columnIndex = col;
                            var targetPos = GetTargetPosition(item.rowIndex, item.columnIndex);
                            item.SetPosition(item.rowIndex, item.columnIndex, targetPos, true);

                            break; // 本次下落完成
                        }
                    }
                }
            }
        }
    }
    
    private void GenerateNewFruits()
    {
        for (var col = 0; col < _currentLevel.gridColumn; col++)
        {
            for (var row = 0; row < _currentLevel.gridRow; row++)
            {
                if (_fruitGrid[row][col] == null)
                {
                    this.SendCommand(new GenerateFruitCommand(row, col));
                    _fruitGrid[row][col] = this.GetModel<IFruitModel>().newFruit;
                    // todo: 从顶部掉落动画（当前原地生成）
                }
            }
        }
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
