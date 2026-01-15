using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class GameController : MonoBehaviour, IController
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
