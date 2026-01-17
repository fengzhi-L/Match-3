using QFramework;

public interface ILevelModel :IModel
{
    LevelData currentLevelData { get; }
    void SetLevelData(int level);
}
