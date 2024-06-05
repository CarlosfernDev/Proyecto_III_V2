using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ODS12Singleton : MinigameParent
{
    public static ODS12Singleton Instance;
    
    public enum gameStage { Stage1, Stage2, Stage3, Stage4 }
    public gameStage currentStage = gameStage.Stage1;
    
    public TimerMinigame gameTimer;

    public UnityEvent OnGarbageDelivered;
    public UnityEvent OnGarbageCreated;

    public Transform playerPickupTransform;
    
    public float currentGarbSpawnTime;
    public float CintaSpeed;
    public int maxGarbage = 10;
    public int currentGarbage = 0;
    public int scoreAdd = 1;
    public int scoreRemove = 1;
    public float timePenalty = 20;
    [Header("TimeStamps")]
    [SerializeField] private float stage1Speed;
    [SerializeField] private float stage2Speed;
    [SerializeField] private float stage3Speed;
    [SerializeField] private float stage4Speed;

    [SerializeField] private float stage1Percentage;
    [SerializeField] private float stage2Percentage;
    [SerializeField] private float stage3Percentage;

    [SerializeField] private float stage1GarbSpawnTime;
    [SerializeField] private float stage2GarbSpawnTime;
    [SerializeField] private float stage3GarbSpawnTime;
    [SerializeField] private float stage4GarbSpawnTime;

    [SerializeField] private List<float> _timeStamps;

    private float _gameTimeThird;
    [SerializeField] private float _currentGameTime;

    public Action PickItemEvent;
    public Action DropItemEvent;

    public Vector3 FinalSpeedBoost;

    public Animator _anim;
    public List<AnimatorOverrideController> AnimatorsBocadillo;

    public AudioSource _TrashInteract;
    public AudioSource CloseAudio;
    public AudioSource OpenAudio;

    protected override void personalAwake()
    {
        Instance = this;
        base.personalAwake();
        OnGarbageDelivered.AddListener(ReduceCurrentGarbageCount);
        OnGarbageCreated.AddListener(IncreaseGarbageCount);
        //_gameTimeThird = gameTimer.TimerValue / 3;
        CalculateTimeStamps();
        currentGarbSpawnTime = stage1GarbSpawnTime;
    }

    private void Update()
    {
        
    }

    protected override void personalStart()
    {
        base.personalStart();
        gameTimer.PreSetTimmer();
    }

    protected override void OnGameStart()
    {
        base.OnGameStart();
        gameTimer.SetTimer();
    }

    private void ReduceCurrentGarbageCount()
    {
        currentGarbage--;
    }

    private void IncreaseGarbageCount()
    {
        currentGarbage++;
    }

    public void CheckStageChange()
    {
        _currentGameTime = gameTimer.GetTimeInFloat();

        if (currentStage == gameStage.Stage4) return;
        if (_currentGameTime < _timeStamps[(int)currentStage])
        {
            currentStage++;
            CheckTimeToSpawn();
        }
    }

    private void CheckTimeToSpawn()
    {
        switch (currentStage)
        {
            case (gameStage.Stage1):
                CintaSpeed = stage1Speed;
                currentGarbSpawnTime = stage1GarbSpawnTime;
                break;

            case (gameStage.Stage2):
                CintaSpeed = stage2Speed;
                currentGarbSpawnTime = stage2GarbSpawnTime;

                // Cambiar fue una implementacion rapida :p
                GameManager.Instance.playerScript.BoostVelocidadPermanente(FinalSpeedBoost.x, FinalSpeedBoost.y, FinalSpeedBoost.z);
                break;

            case (gameStage.Stage3):
                CintaSpeed = stage3Speed;
                currentGarbSpawnTime = stage3GarbSpawnTime;
                break;

            case (gameStage.Stage4):
                CintaSpeed = stage4Speed;
                currentGarbSpawnTime = stage4GarbSpawnTime;
                break;
        }
    }

    private void CalculateTimeStamps()
    {

        _timeStamps[0] = (stage1Percentage / 100) * gameTimer.TimerValue;
        _timeStamps[1] = (stage2Percentage / 100) * gameTimer.TimerValue;
        _timeStamps[2] = (stage3Percentage / 100) * gameTimer.TimerValue;
    }

    public void PickItems()
    {
        if (!GameManager.Instance.playerScript.refObjetoEquipado.TryGetComponent<GarbageScript>(out GarbageScript thisGarbage)) return;

        _anim.runtimeAnimatorController = AnimatorsBocadillo[(int)thisGarbage.thisGarbageType];
        _anim.SetTrigger("On");

        PickItemEvent?.Invoke();
    }

    public void DropItem()
    {
        _anim.SetTrigger("Off");
        DropItemEvent?.Invoke();
    }

    public override void SetResult()
    {
        RankImage.sprite = RankData.timerImageArray[MinigameData.CheckPointsState(Score)].sprite;

        _ScoreText.ChangeText("Score: " + Mathf.Clamp(Score, 0, Score).ToString());

        _txHighScore.text = "High: " + Mathf.Clamp(MinigameData.maxPoints, 0, MinigameData.maxPoints);
    }

    public override void SaveValue()
    {
        if (0 == MinigameData.CheckPointsState(Score)) Score = -1;
        SaveValue(Score);
    }
}
