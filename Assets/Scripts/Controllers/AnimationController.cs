using System;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Action OnAnimationFinished;
    
    public void OnAnimationFinishEvent(string str)
    {
        if (str == "finish")
        {
            OnAnimationFinished?.Invoke();
        }
    }
}
