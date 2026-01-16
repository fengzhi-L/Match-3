using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class UserModel : AbstractModel,IUserModel
{
    public string userId { get; set; }
    public string displayName { get; set; }
    public bool isGuest { get; set; }
    public int currentLevel { get; set; }
    public int totalScore { get; set; }
    public Dictionary<string, int> inventory { get; set; }

    protected override void OnInit()
    {
        userId = "";
        displayName = "玩家";
        isGuest = true;
        currentLevel = 1;
        totalScore = 0;
        inventory = new();
    }

    public void InitializeAsGuest()
    {
        // 生成唯一匿名 ID（基于设备或时间）
        //UserId = SystemInfo.deviceUniqueIdentifier; // 注意：iOS 限制
        // 或使用 GUID（换设备会丢失）
        userId = System.Guid.NewGuid().ToString();
        
        isGuest = true;
        displayName = "游客" + userId.Substring(0, 6);
        LoadLocalData();
    }

    private void LoadLocalData()
    {
        // 从 PlayerPrefs 或本地文件加载
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        totalScore = PlayerPrefs.GetInt("TotalScore", 0);
    }

    public void SaveToLocal()
    {
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        PlayerPrefs.SetInt("TotalScore", totalScore);
        PlayerPrefs.Save();
    }
}