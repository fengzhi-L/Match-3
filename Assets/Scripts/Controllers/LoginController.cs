using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour, IController
{
    public GameObject loginPanel;
    
    public void OnGuestButtonClicked()
    {
        this.GetModel<IUserModel>().InitializeAsGuest();
        this.SendCommand(new LoadSceneAsyncCommand("Level01"));
        loginPanel.SetActive(false);
    }
    
    public void OnGoogleButtonClicked()
    {
        var a = LevelGridLoader.LoadConfig("Level_01_Grid.json");
        foreach (var b in a)
        {
            foreach (var c in b)
            {
                Debug.Log(c.cellType);
            }
        }
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
