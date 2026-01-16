using QFramework;
using UnityEngine;

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

    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
