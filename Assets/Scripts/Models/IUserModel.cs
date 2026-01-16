using System.Collections.Generic;
using QFramework;

public interface IUserModel : IModel
{
    public string userId { get; set; }
    public string displayName { get; set; }
    public bool isGuest { get; set; } 
    public int currentLevel { get; set; } 
    public int totalScore { get; set; } 
    public Dictionary<string, int> inventory { get; set; }  // 道具
    void InitializeAsGuest();
    void SaveToLocal();
}