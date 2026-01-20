using System.Collections.Generic;
using QFramework;

public interface IGameGridModel: IModel
{
    List<List<GridCell>> currentGrid { get;}
    List<List<FruitCell>> currentFruitGrid { get; }
    void SetCurrentGrid(List<List<GridCell>> grid);
}
