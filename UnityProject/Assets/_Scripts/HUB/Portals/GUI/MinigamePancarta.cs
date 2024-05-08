using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamePancarta : LInteractableParent
{
    public GUIAreUSure _Gui;

    public int SceneID;
    public int FadeInID;
    public int FadeOutID;
    public float LoadTime;
    public string PancartaText;

    public Transform PlayerPositionOnReturn;

    public override void Interact()
    {
        base.Interact();
        _Gui.ChangeText(PancartaText);
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

        HubManager.TeleportToThisPosition = PlayerPositionOnReturn.position;

        MySceneManager.Instance.NextScene(SceneID, FadeInID, FadeOutID, LoadTime);
        _Gui._FunctionOnYes -= teleport;
    }
}
