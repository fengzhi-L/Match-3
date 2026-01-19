using QFramework;
using UnityEngine;

public class GetScoreCommand : AbstractCommand
{
    private Vector3 _pos;
    private int _score;

    public GetScoreCommand(Vector3 pos, int score)
    {
        _pos = pos;
        _score = score;
    }

    protected override void OnExecute()
    {
        this.SendEvent<GetScoreEvent>(new() { Position = _pos, Score = _score });
    }
}
