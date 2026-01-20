using QFramework;

public class FruitMoveSuccessCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.SendEvent<FruitMoveSuccessEvent>();
    }
}
