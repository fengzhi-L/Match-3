using System.Collections.Generic;
using QFramework;

public class GameGridModel : AbstractModel, IGameGridModel
{
    public List<List<GridCell>> currentGrid { get; private set; }
    public List<List<FruitCell>> currentFruitGrid { get; private set; }
    
    protected override void OnInit()
    {
        currentGrid = new ();
        currentFruitGrid = new();
    }

    public void SetCurrentGrid(List<List<GridCell>> grid)
    {
        currentGrid = grid;
    }
}
