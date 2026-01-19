using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class FruitUnSelectedCommand : AbstractCommand
{
    private FruitItem _item;

    public FruitUnSelectedCommand(FruitItem item)
    {
        _item = item;
    }

    protected override void OnExecute()
    {
        this.SendEvent<FruitUnSelectedEvent>(new () { FruitItem = _item });
    }
}
