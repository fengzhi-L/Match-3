using QFramework;

public class GenerateFruitCommand : AbstractCommand
{
    private int _row;
    private int _col;

    public GenerateFruitCommand(int row, int col)
    {
        _row = row;
        _col = col;
    }

    protected override void OnExecute()
    {
        this.SendEvent<GenerateFruitEvent>(new () { Row = _row, Column = _col });
    }
}
