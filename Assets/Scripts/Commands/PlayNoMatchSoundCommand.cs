using QFramework;

public class PlayNoMatchSoundCommand :AbstractCommand
{
    protected override void OnExecute()
    {
        this.SendEvent<PlayNoMatchSoundEvent>();
    }
}
