using System;
using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour, IController
{
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

    public void OnButtonClicked(string level)
    {
        SceneManager.LoadScene(level);
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
