using QFramework;
using UnityEngine;

public class MainMenuController : MonoBehaviour, IController
{
    

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
