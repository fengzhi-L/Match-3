using QFramework;

public class GameLoseCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.SendEvent<GameLoseEvent>();
    }
}
