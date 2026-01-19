using System;
using QFramework;
using UnityEngine;

public class AnimationController : MonoBehaviour, IController
{
    public Action<string> animEventCallback;
    
    public void OnAnimationFinishEvent(string str)
    {
        if (animEventCallback != null) animEventCallback(str);
    }
    
    public IArchitecture GetArchitecture()
    {
        return Match3.Interface;
    }
}
