using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class FruitMoveCommand : AbstractCommand
{
    private Vector3 _delta;

    public FruitMoveCommand(Vector3 delta)
    {
        _delta = delta;
    }
    protected override void OnExecute()
    {
        this.SendEvent<FruitMoveEvent>(new() { Delta = _delta });
    }
}
