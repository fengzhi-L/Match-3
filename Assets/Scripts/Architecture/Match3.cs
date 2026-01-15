using QFramework;

public class Match3 : Architecture<Match3>
{
    protected override void Init()
    {
        RegisterModel<IGameGridModel>(new GameGridModel());
        
        RegisterSystem<IGameGridSystem>(new GameGridSystem());
    }
}
