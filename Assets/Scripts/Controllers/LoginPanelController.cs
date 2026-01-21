using QFramework;
using UnityEngine;

public class LoginPanelController : MonoBehaviour, IController
{
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject guestButton;
    [SerializeField] private GameObject googleButton;

    private void Start()
    {
        loginPanel.SetActive(true);
    }

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
