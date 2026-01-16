using QFramework;

public class GameGridSystem : AbstractSystem, IGameGridSystem
{
    private GridCell[,] _grid;
    private LevelData _levelData;
    private System.Random _random;

    public int column => _levelData.gridColumn;
    public int row => _levelData.gridRow;
    
    protected override void OnInit()
    {
        _random = new System.Random();
    }
    
}
