using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AnimatorPapeleraTutorial : MonoBehaviour
{
    public Animator _anim;
    private bool IsOpen;
    public VisualEffect Effect;


    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player") return;

        if (GameManager.Instance.playerScript.refObjetoEquipado == null && IsOpen)
        {
            IsOpen = false;
            _anim.SetBool("IsOpen", IsOpen);

            _anim.SetTrigger("TriggerActionOpen");
        }
        if (GameManager.Instance.playerScript.refObjetoEquipado != null && !IsOpen)
        {
            IsOpen = true;
            _anim.SetBool("IsOpen", IsOpen);

            _anim.SetTrigger("TriggerActionOpen");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;

        if (GameManager.Instance.playerScript.refObjetoEquipado != null)
        {
            IsOpen = false;
            _anim.SetBool("IsOpen", IsOpen);

            _anim.SetTrigger("TriggerActionOpen");
        }
    }

    public void enableVFX()
    {
        Effect.Play();
    }

    public void disableVFX()
    {
        Effect.Stop();
    }
}
