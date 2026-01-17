using System.Collections.Generic;
using QFramework;

public interface IUserModel : IModel
{
    public string userId { get; set; }
    public string displayName { get; set; }
    public bool isGuest { get; set; } 
    BindableProperty<int> currentLevel { get; } 
    public int totalScore { get; set; } 
    public Dictionary<string, int> inventory { get; set; }  // 道具
    void InitializeAsGuest();
    void SaveToLocal();
}