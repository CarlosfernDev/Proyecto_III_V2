using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecolectorBasuraRework : LInteractableParent
{
    public enum collectorType { UNDEFINED, PAPER, PLASTIC, GLASS }
    public collectorType thisCollectorType = collectorType.UNDEFINED;

    private void Update()
    {
        if (thisCollectorType == collectorType.UNDEFINED)
        {
            Debug.LogError("Garbage Collector Type Not Defined");
        }
    }

    public override void Interact()
    {
        base.Interact();

        if (GameManager.Instance.playerScript.refObjetoEquipado == null)
        {
            return;
        }

        if (!GameManager.Instance.playerScript.refObjetoEquipado.TryGetComponent<GarbageScript>(out GarbageScript thisGarbage)) return;

        if (thisGarbage.thisGarbageType.ToString() == thisCollectorType.ToString())
        {
            ODS12Singleton.Instance.AddScore(ODS12Singleton.Instance.scoreAdd);
            ODS12Singleton.Instance.OnGarbageDelivered.Invoke();
            Destroy(thisGarbage.gameObject);
        }
        else
        {
            ODS12Singleton.Instance.RemoveScore(ODS12Singleton.Instance.scoreRemove);
            ODS12Singleton.Instance.gameTimer.RestTime(ODS12Singleton.Instance.timePenalty);
            ODS12Singleton.Instance.OnGarbageDelivered.Invoke();
            Destroy(thisGarbage.gameObject);
        }

        ODS12Singleton.Instance.DropItem();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!other.TryGetComponent(out GarbageScript thisGarbage)) return;
    //    if (thisGarbage.thisGarbageType.ToString() == thisCollectorType.ToString())
    //    {
    //        ODS12Singleton.Instance.AddScore(ODS12Singleton.Instance.scoreAdd);
    //        ODS12Singleton.Instance.OnGarbageDelivered.Invoke();
    //        Destroy(thisGarbage.gameObject);
    //    }
    //    else
    //    {
    //        ODS12Singleton.Instance.RemoveScore(ODS12Singleton.Instance.scoreRemove);
    //        ODS12Singleton.Instance.gameTimer.RestTime(ODS12Singleton.Instance.timePenalty);
    //        ODS12Singleton.Instance.OnGarbageDelivered.Invoke();
    //        Destroy(thisGarbage.gameObject);
    //    }
    //}
}
