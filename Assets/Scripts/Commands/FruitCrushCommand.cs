using QFramework;
using UnityEngine;

public class FruitCrushCommand : AbstractCommand
{
    private FruitType _fruitType;
    private Vector3 _pos;

    public FruitCrushCommand(FruitType fruitType, Vector3 pos)
    {
        _fruitType = fruitType;
        _pos = pos;
    }

    protected override void OnExecute()
    {
        this.SendEvent<FruitCrushEvent>(new() { FruitType = _fruitType, Position = _pos});
    }
}
