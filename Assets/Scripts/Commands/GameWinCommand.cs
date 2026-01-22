using QFramework;

public class GameWinCommand : AbstractCommand
{
    private int _score;
    private int _starCount;

    public GameWinCommand(int score, int starCount)
    {
        _score = score;
        _starCount = starCount;
    }
    protected override void OnExecute()
    {
        this.SendEvent<GameWinEvent>(new() { Score = _score, StarCount = _starCount });
    }
}
