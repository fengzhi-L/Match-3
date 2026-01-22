using QFramework;
using UnityEngine;

public class MainCanvasController : MonoBehaviour, IController
{
    [SerializeField] private GameObject mainPanel;

    public void OnLevelSelectClicked()
    {
        this.SendCommand<PlayButtonClickSoundCommand>();
        this.SendCommand(new LoadSceneAsyncCommand("LevelSelect"));
        mainPanel.SetActive(false);
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
