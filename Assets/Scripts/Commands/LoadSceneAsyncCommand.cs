using QFramework;

public class LoadSceneAsyncCommand : AbstractCommand
{
    private string _sceneName;

    public LoadSceneAsyncCommand(string sceneName)
    {
        _sceneName = sceneName;
    }
    protected override void OnExecute()
    {
        this.SendEvent<LoadSceneAsyncEvent>(new() { SceneName = _sceneName });
    }
}
