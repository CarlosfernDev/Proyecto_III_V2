using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateDisable : MonoBehaviour
{
    public GameManager.GameState EnableState;

    private void Start()
    {
        if (GameManager.Instance == null) return;

        if(GameManager.Instance.state >= EnableState)
            gameObject.SetActive(true);
        else gameObject.SetActive(false);

    }
}
