using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour, IController
{
    public GameObject loginPanel;
    [SerializeField] private GameObject tipPanel;
    
    public void OnGuestButtonClicked()
    {
        this.SendCommand<PlayButtonClickSoundCommand>();
        this.GetModel<IUserModel>().InitializeAsGuest();
        this.SendCommand(new LoadSceneAsyncCommand("Main"));
        loginPanel.SetActive(false);
    }
    
    public void OnGoogleButtonClicked()
    {
        this.SendCommand<PlayButtonClickSoundCommand>();
        tipPanel.SetActive(true);
        Debug.Log("谷歌登录未实现");
    }

    public void OnTipPanelClosed()
    {
        tipPanel.SetActive(false);
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
