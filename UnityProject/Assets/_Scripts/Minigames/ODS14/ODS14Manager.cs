using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ODS14Manager : MinigameParent
{
    public static ODS14Manager Instance;
    public TimerMinigame timer;
    public LivesManager playerLives;

    public UnityEvent reduceLife;
    public UnityEvent garbageHit;
    
    private int _currentLives;
    [SerializeField] private int _garbageLeft;
    private List<GameObject> _floatingGarbage;

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
        garbageHit.AddListener(GarbageHit);
    }
    
    protected override void OnGameStart()
    {
        base.OnGameStart();
        timer.SetTimer();
        
        InitializeGarbageLeft();
        EnableMovement();
    }
    
    public override void OnGameFinish()
    {
        base.OnGameFinish();
        DisableMovement();
        timer.PauseTimer();
    }

    public void AnimalHit()
    {
        playerLives.updateLives.Invoke();
    }

    private void GarbageHit()
    {
        _garbageLeft--;
        if (_garbageLeft <= 0)
        {
            _garbageLeft = 0;
            OnGameFinish();
        }
    }

    public void DisableMovement()
    {
        canMove = false;
    }
    public void EnableMovement()
    {
        canMove = true;
    }

    private void InitializeGarbageLeft()
    {
        _garbageLeft = FindObjectsOfType<FloatingGarbage>().Length;
    }

    private void DecreaseLives()
    {
        _currentLives--;
        if (_currentLives <= 0)
        {
            _currentLives = 0;
            OnGameFinish();
        }
    }

    public override void SetResult()
    {
        Debug.Log("Se hace tranquilo");
        Debug.Log(Score);

        RankImage.sprite = RankData.timerImageArray[MinigameData.CheckPointsState(Score)].sprite;

        _ScoreText.ChangeText(timer.GetTimeInSeconds());

        int minutos = Mathf.FloorToInt(MinigameData.maxPoints / 60);
        int segundos = Mathf.FloorToInt(MinigameData.maxPoints % 60);

        _txHighScore.text = "High: " + string.Format("{0:00}:{1:00}", minutos, segundos/*, milisegundos*/);
    }

    public override void SaveValue()
    {
        if (_garbageLeft != 0)
        {
            SaveValue(-1);
            Score = -1;
            return;
        }
        Score = timer.GetRealTime();
        SaveValue(Score);
    }
}
