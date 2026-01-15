using QFramework;
using UnityEngine;

public class LevelModel : AbstractModel, ILevelModel
{
    private LevelData _currentLevelData;
    protected override void OnInit()
    {
        _currentLevelData = Resources.Load<LevelCollection>($"Data/Levels/LevelCollection").levels[0];
    }

    public LevelData GetCurrentLevelData()
    {
        return _currentLevelData;
    }

    public void SetCurrentLevelData(LevelData levelData)
    {
        _currentLevelData = levelData;
    }
}
