using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamePortal : MonoBehaviour
{
    public GUIAreUSure _Gui;

    public int SceneID;
    public int FadeInID;
    public int FadeOutID;
    public float LoadTime;
    public string PortalText;

    public Transform PlayerPositionOnReturn;

    public float timeReference = 0;

    private void Start()
    {
        timeReference = -99;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (Time.time - timeReference < 10f) return;
        if (collision.tag != "Player") return;

        if (_Gui != null)
        {
            _Gui.ChangeText(PortalText);
            _Gui._FunctionOnYes += teleport;
            _Gui._FunctionOnNo += SetColdown;
            _Gui.EnableMenu();
        }
        else
        {
            teleport();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag != "Player") return;

        timeReference = -99;
    }

    public void teleport()
    {
        if (MySceneManager.Instance == null)
        {
            Debug.LogError("Te falto un scenemanager.");
            return;
        }

        HubManager.TeleportToThisPosition =  PlayerPositionOnReturn.position;

        MySceneManager.Instance.NextScene(SceneID, FadeInID, FadeOutID, LoadTime);
        if (_Gui != null)
        {
            _Gui._FunctionOnYes -= teleport;
            _Gui._FunctionOnNo -= SetColdown;
        }
    }

    public void SetColdown()
    {
        timeReference = Time.time;
        _Gui._FunctionOnNo -= SetColdown;
        _Gui._FunctionOnYes -= teleport;
    }
}
