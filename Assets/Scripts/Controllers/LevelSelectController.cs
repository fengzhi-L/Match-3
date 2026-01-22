using System;
using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour, IController
{
    [SerializeField] private GameObject levelSelectPanel;
    
    [Serializable]
    public struct ButtonPlayerPrefs
    {
        public GameObject gameObject;
        public string playerPrefKey;
    }

    public ButtonPlayerPrefs[] buttons;

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            var score = PlayerPrefs.GetInt(buttons[i].playerPrefKey, 0);
            for (var starIndex = 1; starIndex <= 3; starIndex++)
            {
                var star = buttons[i].gameObject.transform.Find($"star{starIndex}");
                star.gameObject.SetActive(starIndex <= score);
            }
        }
    }

    public void OnButtonClicked(int level)
    {
        this.GetModel<IUserModel>().currentLevel.Value = level;
        this.SendCommand(new LoadSceneAsyncCommand($"Level{level}"));
        levelSelectPanel.SetActive(false);
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
