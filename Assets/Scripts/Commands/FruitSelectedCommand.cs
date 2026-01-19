using QFramework;

public class FruitSelectedCommand : AbstractCommand
{
    private FruitItem _item;

    public FruitSelectedCommand(FruitItem item)
    {
        _item = item;
    }

    protected override void OnExecute()
    {
        this.SendEvent<FruitSelectedEvent>(new() { FruitItem = _item});
    }
}
