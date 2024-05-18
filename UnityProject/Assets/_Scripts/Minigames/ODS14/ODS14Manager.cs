using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ODS14Manager : MinigameParent
{
    public static ODS14Manager Instance;
    public TimerMinigame timer;
    public LivesManager playerLives;
    public TMP_Text scoreText;
    
    public List<GameObject> _floatingGarbage;
    private FloatingGarbage[] _startingGarbage;

    public UnityEvent reduceLife;
    public UnityEvent garbageHit;
    
    private int _currentLives;
    [SerializeField] private int _garbageLeft;

    public float extraTime = 5f;
    
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
        //garbageHit.AddListener(GarbageHit());
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

    public void GarbageHit(FloatingGarbage hitGarbage)
    {
        if (_floatingGarbage.Contains(hitGarbage.transform.parent.gameObject))
        {
            _floatingGarbage.Remove(hitGarbage.transform.parent.gameObject);
        }
        _garbageLeft--;
        timer.AddTime(extraTime);
        if (_garbageLeft <= 0)
        {
            _garbageLeft = 0;
            scoreText.text = _garbageLeft.ToString();
            OnGameFinish();
        }
        scoreText.text = _garbageLeft.ToString();
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
        _startingGarbage = FindObjectsOfType<FloatingGarbage>();
        foreach (var garbage in _startingGarbage)
        {
            _floatingGarbage.Add(garbage.transform.parent.gameObject);
        }
        _garbageLeft = _floatingGarbage.Count;
        scoreText.text = _garbageLeft.ToString();
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

        if(_currentLives == 0)
        {
            _ScoreText.Pretext = null;
            _ScoreText.ChangeText("You have destroyed the marine fauna");
        }
        else
        {
            _ScoreText.ChangeText(timer.GetTimeInSeconds());
        }


        int minutos = Mathf.FloorToInt(Mathf.Clamp(MinigameData.maxPoints, 0, MinigameData.maxPoints) / 60);
        int segundos = Mathf.FloorToInt(Mathf.Clamp(MinigameData.maxPoints, 0, MinigameData.maxPoints) % 60);

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
        Score = (int)timer.Value;
        SaveValue(Score);
    }
}
