using QFramework;

public interface ILevelModel :IModel
{
    LevelData GetCurrentLevelData();
    void SetCurrentLevelData(LevelData levelData);
}
