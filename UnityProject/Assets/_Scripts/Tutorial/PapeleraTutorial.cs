using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapeleraTutorial : LInteractableParent
{
    public TutorialPortal portal;

    public override void Interact()
    {
        if (GameManager.Instance.playerScript.refObjetoEquipado == null)
        {
            return;
        }

        if (!GameManager.Instance.playerScript.refObjetoEquipado.TryGetComponent<BasuraTutorial>(out BasuraTutorial thisGarbage)) return;

        Destroy(thisGarbage.gameObject);
        portal.EnablePortal();

        TutorialManager.Instance.OnDropItem?.Invoke();

        GameManager.Instance.playerScript.isEquipado = false;
        Unhover();
    }

    public override void Hover()
    {
        if (!GameManager.Instance.playerScript.isEquipado) return;
        base.Hover();
    }
}
