using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalFinal : MonoBehaviour
{
    public int SceneID;
    public int FadeInID;
    public int FadeOutID;
    public float LoadTime;
    public string PortalText;

    public Transform PlayerPositionOnReturn;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag != "Player") return;

        teleport();
    }

    public void teleport()
    {
        GameManagerSergio.Instance.youWin = true;
        GameManagerSergio.Instance.EnseñarPantallaFinal();
    }
}
