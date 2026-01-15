using QFramework;

public interface IGameGridModel: IModel
{
    GridCell[,] GetCurrentGrid();
    void SetCurrentGrid(int width, int height);
}
