using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour, IController
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGuestButtonClicked()
    {
        this.GetModel<IUserModel>().InitializeAsGuest();
        SceneManager.LoadScene(1);
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
