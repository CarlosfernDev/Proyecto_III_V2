using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jurado : MonoBehaviour
{
    public JuradoScriptable JuradoData;
    public Animator _animator;
    public Image _image;

    public void ShowScore(int value)
    {
        _animator.SetTrigger(JuradoData.JuradoList[value].AnimationTrigger);
        ChangeImage(value);
    }

    public void ChangeImage(int value)
    {
        _image.sprite = JuradoData.JuradoList[value].sprite;
    }
}
