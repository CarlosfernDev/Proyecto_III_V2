using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableInputView : MonoBehaviour
{
    public Animator KeyAnimator;
    public bool IsEnable = false;

    public void StartAnimation()
    {
        if (IsEnable) return;
        IsEnable = true;
        KeyAnimator.SetBool("IsEnable", IsEnable);
        KeyAnimator.SetTrigger("Enable");
    }

    public void CloseAnimation()
    {
        if (!IsEnable) return;
        IsEnable = false;
        KeyAnimator.SetBool("IsEnable", IsEnable);
        KeyAnimator.SetTrigger("Enable");
    }
}
