using System.Collections.Generic;
using QFramework;

public class GameGridModel : AbstractModel, IGameGridModel
{
    public List<List<GridCell>> currentGrid { get; set; }
    public List<List<FruitItem>> currentFruitGrid { get; set; }

    protected override void OnInit()
    {
        currentGrid = new ();
        currentFruitGrid = new();
    }
}
