using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    public static Vector3 TeleportToThisPosition;
    public GameManager.GameState CustomState;

    private void Awake()
    {
        GameManager.Instance.isPlaying = true;

        // GameManager.Instance.state = GameManager.GameState.PostGame;
        //GameManager.Instance.UpdateState();
        GameManager.Instance.NextState((int)CustomState);
    }

    private void Start()
    {
        Debug.Log("Se intento tpear " + TeleportToThisPosition);
        if(!Vector3.Equals( TeleportToThisPosition, Vector3.zero))
        {
            TestInputs player = GameManager.Instance.playerScript;
            player.rb.position = TeleportToThisPosition;
            TeleportToThisPosition = Vector3.zero;
        }
    }
}
