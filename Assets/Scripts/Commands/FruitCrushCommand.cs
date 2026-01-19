using QFramework;
using UnityEngine;

public class FruitCrushCommand : AbstractCommand
{
    private Vector3 _pos;

    public FruitCrushCommand(Vector3 pos)
    {
        _pos = pos;
    }

    protected override void OnExecute()
    {
        this.SendEvent<FruitCrushEvent>(new() { Position = _pos});
    }
}
