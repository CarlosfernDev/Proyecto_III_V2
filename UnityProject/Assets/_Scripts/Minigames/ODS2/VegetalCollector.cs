using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetalCollector : LInteractableParent
{
    public override void Interact()
    {
        if (!GameManager.Instance.playerScript.isEquipado) return;

        if (!GameManager.Instance.playerScript.refObjetoEquipado.TryGetComponent<Vegetal>(out Vegetal script)) return;

        base.Interact();

        ODS2Singleton.Instance.AddScore(ODS2Singleton.Instance.ScoreCollectingDone);
        ODS2Singleton.Instance.OnVegetalDone();

        
        Destroy(script.gameObject);
        GameManager.Instance.playerScript.isEquipado = false;
    }
}
