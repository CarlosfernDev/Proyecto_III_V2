using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VegetalCollector : LInteractableParent
{
    public Animator _animatorGps;

    public VisualEffect _pickeablevfx;

    public override void Interact()
    {
        if (!GameManager.Instance.playerScript.isEquipado) { return; }

        if (!GameManager.Instance.playerScript.refObjetoEquipado.TryGetComponent<Vegetal>(out Vegetal script)) return;

        base.Interact();

        ODS2Singleton.Instance.AddScore(ODS2Singleton.Instance.ScoreCollectingDone);
        ODS2Singleton.Instance.OnVegetalDone();

        ODS2Singleton.Instance.DisableAllGps();
        
        Destroy(script.gameObject);
        GameManager.Instance.playerScript.isEquipado = false;
    }

    public void EnableGps()
    {
        _animatorGps.SetTrigger("On");
        _pickeablevfx.Play();
    }

    public void DisableGps()
    {
        _animatorGps.SetTrigger("Off");
        _pickeablevfx.Stop();
    }

    public override void Hover()
    {
        if (!GameManager.Instance.playerScript.isEquipado) return;
        base.Hover();
    }
}
