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
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag != "Player") return;

        _Gui._FunctionOnYes += teleport;
        _Gui.EnableMenu();
    }

    public void teleport()
    {
        if (MySceneManager.Instance == null)
        {
            Debug.LogError("Te falto un scenemanager.");
            return;
        }

        MySceneManager.Instance.NextScene(SceneID, FadeInID, FadeOutID, LoadTime);
        _Gui._FunctionOnYes -= teleport;
    }
}
