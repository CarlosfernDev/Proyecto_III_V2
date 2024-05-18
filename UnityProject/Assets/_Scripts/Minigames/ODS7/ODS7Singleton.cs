using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(TimerMinigame))]
public class ODS7Singleton : MinigameParent
{
    public static ODS7Singleton Instance;
    
    public Timer _txTime;

    public Animator _gpsAnimator;

    public TimerMinigame timer;

    [Header("Player Variables")]
    public TestInputs player;
    public EquipableRedTest playerNet;

    [Header("Spawner Variables")] 
    public List<CloudSpawner> spawnersDisablingList;
    public List<CloudSpawner> enabledSpawners;
    public GameObject CloudPrefab;
    
    public float timeFabricaDestroy;
    public float timeCloudSpawn;
    public float timeToNewCloudCall;
    public float transformTimeIncrease = 1.5f;

    [Header("Cloud Variables")]
    public List<CloudAI> activeClouds;
    public List<CloudAI> inactiveClouds;
    public Transform EnemyEmptyParent;
    public int maxClouds;

    private bool _allGeneratorsDisabled;
    private bool _allCloudsCaptured;

    [Header("Rewards")] 
    public float bonusTime;

    [Header("UI Elements")]
    public Image[] generatorStatusSprites;
    public Animator generatorStatusAnim;
    public Sprite activatedGenerator;
    public Sprite deactivatedGenerator;
    public TMP_Text cloudCount; 


    [Header("UI Bubbles")]
    public Animator anim;
    public AnimatorOverrideController animatorBocadillo;

    #region Override Methods

    protected override void personalAwake()
    {
        base.personalAwake();
        activeClouds = new List<CloudAI>();
        spawnersDisablingList = new List<CloudSpawner>();
        enabledSpawners = new List<CloudSpawner>();
        Instance = this;
    }

    protected override void personalStart()
    {
        GameManager.Instance.playerScript.DisablePlayer();
        base.personalStart();
        playerNet = FindObjectOfType<EquipableRedTest>();
        player = GameManager.Instance.playerScript;
        timer.PreSetTimmer();
    }

    protected override void OnGameStart()
    {
        GameManager.Instance.playerScript.sloopyMovement = true;
        base.OnGameStart();
        timer.SetTimer();
    }

    public override void OnGameFinish()
    {
        timer.PauseTimer();
        GameManager.Instance.playerScript.DisablePlayer();
        GameManager.Instance.playerScript.enabled = false;
        base.OnGameFinish();
    }

    #endregion

    #region Main Methods

    private void Update() { }

    #endregion

    #region OnEnable And OnDisable

    private void OnEnable()
    {
        ODS7Actions.OnSpawnerDisabled += CheckWinLose;
        ODS7Actions.OnCloudDelivered += DeliverCloud;
        ODS7Actions.OnSpawnerDisabled += SpawnerDisabled;
        ODS7Actions.OnCloudSpawned += UpdateCloudAmount;
    }

    private void OnDisable()
    {
        ODS7Actions.OnSpawnerDisabled -= CheckWinLose;
        ODS7Actions.OnCloudDelivered -= DeliverCloud;
        ODS7Actions.OnSpawnerDisabled -= SpawnerDisabled;
        ODS7Actions.OnCloudSpawned -= UpdateCloudAmount;
    }

    #endregion

    #region UI Functions

    public void UpdateSpawnerUI()
    {
        for (int i = 0; i < generatorStatusSprites.Length; i++)
        {
            if (generatorStatusSprites[i].sprite == activatedGenerator)
            {
                generatorStatusSprites[i].sprite = deactivatedGenerator;
                break;
            }
        }

        generatorStatusAnim.SetInteger("LifeValue", Mathf.Abs(enabledSpawners.Count - 2));
        generatorStatusAnim.SetTrigger("Animate");
    }

    private void UpdateCloudAmount()
    {
        cloudCount.text = (activeClouds.Count + inactiveClouds.Count).ToString();
    }

    #endregion

    #region Cloud Functions

    public void DisableCloud(CloudAI cloudToDisable)
    {
        if (!activeClouds.Contains(cloudToDisable)) return;
        activeClouds.Remove(cloudToDisable);
        inactiveClouds.Add(cloudToDisable);
        _gpsAnimator.SetTrigger("On");
    }

    public void DestroyCloud(CloudAI cloudToDestroy)
    {
        if (cloudToDestroy.isActiveAndEnabled)
        {
            cloudToDestroy.ResetMovement();
        }

        if (cloudToDestroy.targetCloudSpawner)
            cloudToDestroy.targetCloudSpawner.TargetAI = null;

        if (activeClouds.Contains(cloudToDestroy))
        {
            activeClouds.Remove(cloudToDestroy);
            Destroy(cloudToDestroy.BaseObject);
        }
        else
        {
            inactiveClouds.Remove(cloudToDestroy);
            Destroy(cloudToDestroy.BaseObject);
        }
    }

    public void CaptureCloud()
    {
        if (!playerNet.isCloudCaptured) return;
        anim.runtimeAnimatorController = animatorBocadillo;
        anim.SetTrigger("On");
    }

    public void DeliverCloud()
    {
        anim.SetTrigger("Off");
        _gpsAnimator.SetTrigger("Off");
        player.BoostVelocidad(10f, 20f, 0.9f, 5f);
        UpdateCloudAmount();
        CheckWinLose();
    }

    private CloudAI FindNearestActiveCloud(Vector3 factoryPosition)
    {
        CloudAI firstClosest = null;
        float closestDistSqr = Mathf.Infinity;

        foreach (CloudAI cloud in activeClouds)
        {
            Vector3 cloudPosition = cloud.transform.position;
            Vector3 directionToCloud = cloudPosition - factoryPosition;
            float dSqrToCloud = directionToCloud.sqrMagnitude;
            if (dSqrToCloud < closestDistSqr)
            {
                closestDistSqr = dSqrToCloud;
                firstClosest = cloud;
            }
        }

        return firstClosest;
    }

    #endregion

    #region Spawner Functions

    public void RequestReinforcements(CloudSpawner affectedSpawner)
    {
        if (affectedSpawner.TargetAI != null) return;

        Transform point = affectedSpawner.centerPoint;
        CloudAI cloudCandidate = FindNearestActiveCloud(point.position);

        if (cloudCandidate.targetCloudSpawner != null) return;

        affectedSpawner.TargetAI = cloudCandidate;
        cloudCandidate.targetCloudSpawner = affectedSpawner;
        affectedSpawner.TargetAI.ReturnToBase(point);
    }

    private void SpawnerDisabled()
    {
        timer.AddTime(bonusTime);
        UpdateSpawnerUI();
        CheckWinLose();
    }

    #endregion

    #region Score Functions

    public override void SetResult()
    {
        RankImage.sprite = RankData.timerImageArray[MinigameData.CheckPointsState(Score)].sprite;

        if (Score == -1)
        {
            _ScoreText.Pretext = null;
            _ScoreText.Preset = null;
            if (enabledSpawners.Count > 0 && (activeClouds.Count + inactiveClouds.Count > 0))
            {
                _ScoreText.ChangeText("Power plants and clouds remaining");
            }
            else if (enabledSpawners.Count > 0)
            {
                _ScoreText.ChangeText("You missed some power plants!");
            }
            else if ((activeClouds.Count + inactiveClouds.Count) > 0)
            {
                _ScoreText.ChangeText("You didn't catch all the pollution!");
            }
        }
        else
        {
            int minutosScore = Mathf.FloorToInt(Mathf.Clamp(Score, 0, Score) / 60);
            int segundosScore = Mathf.FloorToInt(Mathf.Clamp(Score, 0, Score) % 60);

            _ScoreText.ChangeText(string.Format("{0:00}:{1:00}", minutosScore, segundosScore));
        }

        int minutos = Mathf.FloorToInt(Mathf.Clamp(MinigameData.maxPoints, 0, MinigameData.maxPoints) / 60);
        int segundos = Mathf.FloorToInt(Mathf.Clamp(MinigameData.maxPoints, 0, MinigameData.maxPoints) % 60);

        _txHighScore.text = "High: " + string.Format("{0:00}:{1:00}", minutos, segundos/*, milisegundos*/);
    }

    public override void SaveValue()
    {
        // CAMBIA ESTO SI TOCAS ALGO DE COMO CONTAR NUBES O FABRICAS
        if (enabledSpawners.Count > 0 || ((activeClouds.Count + inactiveClouds.Count) > 0)) Score = -1;
        else Score = (int)timer.Value;

        SaveValue(Score);
    }

    private void CheckWinLose()
    {
        if (enabledSpawners.Count <= 0) _allGeneratorsDisabled = true;
        if (activeClouds.Count + inactiveClouds.Count <= 0) _allCloudsCaptured = true;
        if (_allGeneratorsDisabled && _allCloudsCaptured) OnGameFinish();
    }

    #endregion
}
