using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableInputViewONTRIGGER : EnableInputView
{
    public bool CollisionEnable = true;
    private void OnTriggerEnter(Collider other)
    {
        if (!CollisionEnable) return;

        if(other.tag == "Player")
        {
            StartAnimation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!CollisionEnable) return;
        if (other.tag == "Player")
        {
            CloseAnimation();
        }
    }
}
