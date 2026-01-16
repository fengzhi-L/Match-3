using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class FruitModel : AbstractModel, IFruitModel
{
    public FruitItem currentSelectedFruit { get; set; }
    
    protected override void OnInit()
    {
        currentSelectedFruit = null;
    }
}
