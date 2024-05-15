using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateDisable : MonoBehaviour
{
    public GameManager.GameState EnableState;
    public bool WantToDisable = false;

    private void Start()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.state >= EnableState)
        {
            gameObject.SetActive(!WantToDisable);
        }
        else
        {
            gameObject.SetActive(WantToDisable);
        }

    }
}
