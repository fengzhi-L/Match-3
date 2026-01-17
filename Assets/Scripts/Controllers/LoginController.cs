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

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
