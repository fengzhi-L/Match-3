using QFramework;

public class FruitSelectedCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.SendEvent<FruitSelectedEvent>();
    }
}
