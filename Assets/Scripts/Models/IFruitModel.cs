using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public interface IFruitModel : IModel
{
    FruitItem currentSelectedFruit { get; set; }
}
