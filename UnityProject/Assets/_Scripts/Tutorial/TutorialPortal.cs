using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPortal : MonoBehaviour
{
    public Animator _anim;
    public GameObject go;

    private void Start()
    {
        go.SetActive(false);
    }

    public void EnablePortal()
    {
        go.SetActive(true);
        _anim.SetTrigger("Start");
    }
}
