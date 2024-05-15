using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateDisable : MonoBehaviour
{
    public GameManager.GameState EnableState;
    public bool WantToDisable = false;
    public GameObject GameObjectAffected;

    private void Start()
    {
        if(GameObjectAffected == null)
            GameObjectAffected = gameObject;

        if (GameManager.Instance == null) return;

        if (GameManager.Instance.state >= EnableState)
        {
            GameObjectAffected.SetActive(!WantToDisable);
        }
        else
        {
            GameObjectAffected.SetActive(WantToDisable);
        }

    }
}
