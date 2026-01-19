using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public interface IFruitModel : IModel
{
    FruitItem currentSelectedFruit { get; set; }
    List<List<FruitItem>> fruitGrid { get; set; }
    FruitItem newFruit { get; set; }
}
