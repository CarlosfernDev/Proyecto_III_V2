using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class KillPlayer : MonoBehaviour
{
    private Coroutine _cor;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.transform.position = GameManagerSergio.Instance.actualSpawnPoint;
            GameManager.Instance.playerScript.ResetInputs();
        }
    }
}
