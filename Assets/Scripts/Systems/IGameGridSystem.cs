using QFramework;

public interface IGameGridSystem : ISystem
{
    void SpawnGrid();
    void FillGridWithCell(CellType type);
}
