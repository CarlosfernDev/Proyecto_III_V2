using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityDialogue : MonoBehaviour
{
    public Animator _anim;
    private bool _IsEnabled = false;

    private void Start()
    {
        EnableAnimation();
    }

    public void EnableAnimation()
    {
        if (_IsEnabled) return;
        _anim.SetTrigger("On");
        _IsEnabled = true;
    }

    public void DisableAnimation()
    {
        if (!_IsEnabled) return;
        _anim.SetTrigger("Off");
        _IsEnabled = false;
    }
}
