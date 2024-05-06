using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ODS14Manager : MinigameParent
{
    public static ODS14Manager Instance;
    public TimerMinigame timer;
    public LivesManager playerLives;

    public UnityEvent reduceLife;
    
    private int _currentLives;

    public bool canMove;
    
    protected override void personalAwake()
    {
        Instance = this;
        base.personalAwake();
        canMove = false;
    }

    protected override void personalStart()
    {
        base.personalStart();
        timer.PreSetTimmer();
        _currentLives = playerLives.playerLivesSprites.Length;
        reduceLife.AddListener(DecreaseLives);
    }
    
    protected override void OnGameStart()
    {
        base.OnGameStart();
        timer.SetTimer();
        EnableMovement();
    }
    
    public override void OnGameFinish()
    {
        base.OnGameFinish();
        DisableMovement();
    }

    public void AnimalHit()
    {
        playerLives.updateLives.Invoke();
    }

    public void DisableMovement()
    {
        canMove = false;
    }
    public void EnableMovement()
    {
        canMove = true;
    }

    private void DecreaseLives()
    {
        _currentLives--;
        if (_currentLives <= 0)
        {
            _currentLives = 0;
            OnGameFinish();
            return;
        }
    }
}
