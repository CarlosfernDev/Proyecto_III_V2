using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
    public Image[] playerLivesSprites;
    public Sprite fullLife;
    public Sprite emptyLife;

    public UnityEvent updateLives;

    public int playerLives = 3;
    
    private void Awake()
    {
        updateLives.AddListener(RemoveLives);
    }

    public void RemoveLives()
    {
        for (int i = 0; i < playerLivesSprites.Length; i++)
        {
            if (playerLivesSprites[i].sprite == fullLife)
            {
                playerLivesSprites[i].sprite = emptyLife;
                playerLives--;
                ODS14Manager.Instance.reduceLife.Invoke();
                break;
            }
        }
    }
}
