using QFramework;

public class Match3 : Architecture<Match3>
{
    protected override void Init()
    {
        RegisterModel<IUserModel>(new UserModel());
        RegisterModel<ILevelModel>(new LevelModel());
        RegisterModel<IGameGridModel>(new GameGridModel());
        RegisterModel<IFruitModel>(new FruitModel());
        RegisterModel<IMissionModel>(new MissionModel());
        
        RegisterSystem<IGameGridSystem>(new GameGridSystem());
    }
}
