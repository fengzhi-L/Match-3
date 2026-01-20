using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class LevelGridLoader
{
    private const string CONFIG_PATH = "Config/game_config.json";
    
    public static List<List<GridCell>> LoadConfig(string gridPath)
    {
        string path = Path.Combine(Application.streamingAssetsPath, gridPath);
        
#if UNITY_ANDROID && !UNITY_EDITOR
            // 使用UnityWebRequest加载
            return LoadConfigAndroid(path);
#else
        string json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<List<List<GridCell>>>(json);
#endif
    }
    
//     public void SaveConfig(GameConfig config)
//     {
//         // 只能在某些平台写入
// #if UNITY_STANDALONE || UNITY_EDITOR
//         string path = Path.Combine(Application.streamingAssetsPath, CONFIG_PATH);
//         string json = JsonUtility.ToJson(config, true);
//         File.WriteAllText(path, json);
// #endif
//     }
}
