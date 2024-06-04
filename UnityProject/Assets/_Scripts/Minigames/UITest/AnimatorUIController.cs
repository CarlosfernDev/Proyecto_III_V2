using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorUIController : MonoBehaviour
{
    [SerializeField] private Animator showButtonAnimator;
    [SerializeField] private bool pop = false;
    void Start()
    {
        PopIn();
        PopOut();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            PopIn();
        }

        if (Input.GetKey(KeyCode.T))
        {
            PopOut();
        }
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
