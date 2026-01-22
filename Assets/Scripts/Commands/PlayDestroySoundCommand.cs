using QFramework;

public class PlayDestroySoundCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.SendEvent<PlayDestroySoundEvent>();
    }
}
