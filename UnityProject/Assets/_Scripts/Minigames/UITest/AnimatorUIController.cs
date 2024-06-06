using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorUIController : MonoBehaviour
{
    [SerializeField] private Animator showButtonAnimator;
    [SerializeField] private bool pop = false;
    [SerializeField] private bool estoyactivo = false;
    void Start()
    {
        if (estoyactivo)
        {
            PopIn();
            PopOut();
        }
        else
        {
            PopIn();
        }
       
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    void PlayIdle()
    {
        //
    }

    public void PopOut()
    {
        if (pop)
        {
            return;
        }
        showButtonAnimator.SetBool("ImPopping", true);
        pop = true;
        showButtonAnimator.SetTrigger("StartPop");
    }

    public void PopIn()
    {
        if (!pop)
        {
            return;
        }
        showButtonAnimator.SetBool("ImPopping", false);
        pop = false;
        showButtonAnimator.SetTrigger("StartPop");

    }
}
