using System;
using JetBrains.Annotations;
using UnityEngine;

public class AnimatorCallback : MonoBehaviour
{
    public Action Callback;

    [UsedImplicitly]
    public void OnAnimationFinished()
    {
        Callback?.Invoke();
    }
}
