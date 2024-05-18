using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableInputViewONTRIGGER : EnableInputView
{
    public bool CollisionEnable = true;
    private void OnTriggerEnter(Collider other)
    {
        if (!CollisionEnable) return;

        // Cambiar en otra build
        if (ODS7Singleton.Instance.playerNet.isCloudCaptured) return; 
        if (other.tag == "Player")
        {
            StartAnimation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!CollisionEnable) return;

        // Cambiar en otra build
        if (ODS7Singleton.Instance.playerNet.isCloudCaptured) return;
        if (other.tag == "Player")
        {
            CloseAnimation();
        }
    }
}
