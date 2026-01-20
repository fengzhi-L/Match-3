using QFramework;
using UnityEngine;

public class LevelModel : AbstractModel, ILevelModel
{
    public LevelData currentLevelData { get; private set; }

    protected override void OnInit()
    {
        currentLevelData = Resources.Load<LevelCollection>($"Data/Levels/LevelCollection").levels[0];
    }

    public void SetLevelData(int level)
    {
        Debug.Log($"当前关卡{level}");
        currentLevelData = Resources.Load<LevelCollection>($"Data/Levels/LevelCollection").levels[level-1];
    }
    
}
