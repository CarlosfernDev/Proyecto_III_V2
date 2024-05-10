using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    public static Vector3 TeleportToThisPosition;

    private void Start()
    {
        GameManager.Instance.isPlaying = true;

        Debug.Log("Se intento tpear " + TeleportToThisPosition);
        if(!Vector3.Equals( TeleportToThisPosition, Vector3.zero))
        {
            TestInputs player = GameManager.Instance.playerScript;
            player.rb.position = TeleportToThisPosition;
            TeleportToThisPosition = Vector3.zero;
        }
    }
}
