using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ODS14Manager : MinigameParent
{
    public static ODS14Manager Instance;
    public TimerMinigame timer;
    public LivesManager playerLives;
    
    protected override void personalAwake()
    {
        Instance = this;
        base.personalAwake();
    }

    protected override void personalStart()
    {
        base.personalStart();
        timer.PreSetTimmer();
    }
    
    protected override void OnGameStart()
    {
        base.OnGameStart();
        timer.SetTimer();
    }

    public void AnimalHit()
    {
        playerLives.UpdateLives.Invoke();
    }
}
