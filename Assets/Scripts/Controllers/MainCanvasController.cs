using QFramework;
using UnityEngine;

public class MainCanvasController : MonoBehaviour, IController
{
    [SerializeField] private GameObject levelSelectButton;

    public void OnLevelSelectClicked()
    {
        this.SendCommand(new LoadSceneAsyncCommand("LevelSelect"));
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
