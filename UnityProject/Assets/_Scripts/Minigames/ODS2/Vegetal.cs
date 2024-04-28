using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegetal : MonoBehaviour, Iequipable
{
    public Granjas myFarm;


    public void UseEquipment()
    {
        
    }

    public void SetEquipableToPlayer()
    {
        if (GameManager.Instance.playerScript.refObjetoEquipado != null) 
        {
            return;
        }
        GameManager.Instance.playerScript.isEquipado = true;
        GameManager.Instance.playerScript.refObjetoEquipado = transform.gameObject;

        transform.position = GameManager.Instance.playerScript.positionEquipable.transform.position;
        transform.rotation = GameManager.Instance.playerScript.positionEquipable.rotation;
        transform.localScale = GameManager.Instance.playerScript.positionEquipable.localScale;
        transform.parent = GameManager.Instance.playerScript.positionEquipable.transform;
    }
}
