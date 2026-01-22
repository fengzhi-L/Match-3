using QFramework;

public class PlayButtonClickSoundCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.SendEvent<PlayButtonClickSoundEvent>();
    }
}
