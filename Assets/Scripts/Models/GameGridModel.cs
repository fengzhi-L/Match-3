using QFramework;

public class GameGridModel : AbstractModel, IGameGridModel
{
    private GridCell[,] _currentGrid;
    protected override void OnInit()
    {
        _currentGrid = new GridCell[0, 0];
    }

    public GridCell[,] GetCurrentGrid()
    {
        return _currentGrid;
    }

    public void SetCurrentGrid(int width, int height)
    {
        _currentGrid = new GridCell[width, height];
    }
}
