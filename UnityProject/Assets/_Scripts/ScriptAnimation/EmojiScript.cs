using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class EmojiScript : MonoBehaviour
{
    public Animator _anim;

    [YarnCommand("Emoji")]
    public void Emoji(int value)
    {
        _anim.SetTrigger(value);
    }
}
