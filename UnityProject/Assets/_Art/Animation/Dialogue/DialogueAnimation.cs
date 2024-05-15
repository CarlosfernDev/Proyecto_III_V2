using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimation : MonoBehaviour
{
    bool Talking = false;

    public Animator _anim;
    public void StartTalking()
    {
        if (Talking) return;

        _anim.SetTrigger("Talk");
        Talking = true;
    }

    public void StopTalking()
    {
        if (!Talking) return;
        _anim.SetTrigger("StopTalk");
        Talking = false;
    }
}
