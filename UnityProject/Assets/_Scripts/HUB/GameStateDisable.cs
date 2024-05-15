using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateDisable : MonoBehaviour
{
    public GameManager.GameState EnableState;
    public bool WantToDisable;
    public GameObject GameObjectAffected;

    private void Start()
    {
        if (GameObjectAffected == null)
            GameObjectAffected = gameObject;

        if (GameManager.Instance == null) return;

        if (GameManager.Instance.state >= EnableState)
        {
            if (!WantToDisable)
            {
                GameObjectAffected.SetActive(true);
            }
            else
            {
                GameObjectAffected.SetActive(false);
            }
        }
        else
        {
            if (!WantToDisable)
            {
                GameObjectAffected.SetActive(false);
            }
            else
            {
                GameObjectAffected.SetActive(true);
            }
        }

    }
}
