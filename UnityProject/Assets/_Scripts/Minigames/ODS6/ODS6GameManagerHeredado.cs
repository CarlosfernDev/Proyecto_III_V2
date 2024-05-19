using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ODS6GameManagerHeredado : MinigameParent
{
    public static ODS6GameManagerHeredado instance;



    [SerializeField] public TimerMinigame timer;
    [SerializeField] public bool youWin = false;


    protected override void personalAwake()
    {
        base.personalAwake();
        instance = this;
    }

    protected override void OnGameStart()
    {
        base.OnGameStart();
        timer.SetTimer();
    }


    public void ActivateTimer()
    {
        timer.SetTimer();
    }
    public void EnseñarPantallaFinal()
    {
        OnGameFinish();
    }
    public override void OnGameFinish()
    {
        timer.PauseTimer();
        base.OnGameFinish();
    }
    public override void SetResult()
    {
        RankImage.sprite = RankData.timerImageArray[MinigameData.CheckPointsState(Score)].sprite;

        _ScoreText.ChangeText(timer.GetTimeInSeconds());

        int minutos = Mathf.FloorToInt(Mathf.Clamp(MinigameData.maxPoints, 0, MinigameData.maxPoints) / 60);
        int segundos = Mathf.FloorToInt(Mathf.Clamp(MinigameData.maxPoints, 0, MinigameData.maxPoints) % 60);

        _txHighScore.text = "High: " + string.Format("{0:00}:{1:00}", minutos, segundos/*, milisegundos*/);
    }

    public override void SaveValue()
    {
        if (youWin)
        {
            Score = timer.GetRealTime();
            SaveValue(Score);
        }
        else
        {
            Score = -1;
            SaveValue(Score);
        }

    }
}
