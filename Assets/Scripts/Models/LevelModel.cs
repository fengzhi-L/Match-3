using QFramework;
using UnityEngine;

public class LevelModel : AbstractModel, ILevelModel
{
    public LevelData currentLevelData { get; set; }

    protected override void OnInit()
    {
        currentLevelData = Resources.Load<LevelCollection>($"Data/Levels/LevelCollection").levels[0];
    }
    
}
