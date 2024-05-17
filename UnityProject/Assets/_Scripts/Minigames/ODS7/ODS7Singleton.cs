using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(TimerMinigame))]
public class ODS7Singleton : MinigameParent
{
    public Timer _txTime;

    public static ODS7Singleton Instance;

    public TimerMinigame timer;

    [Header("Fabricas Variables")] public List<CloudSpawner> spawnersDisablingList;
    public List<CloudSpawner> enabledSpawners;
    public List<CloudAI> enabledCloudList;
    public List<CloudAI> disabledCloudList;
    public GameObject CloudPrefab;

    private EquipableRedTest _playerNet;

    public float timeFabricaDestroy;
    public float[] timeCloudRestoration;
    public float timeCloudSpawn;
    public float timeToNewCloudCall;
    public float transformTimeIncrease = 1.5f;
    public int maxClouds;

    private bool _allGeneratorsDisabled;
    private bool _allCloudsCaptured;

    [Header("Factory Disabled")]
    public float tryintervalFix;
    public int ChanceBase;
    private float TimeTryReference;

    [Header("Punto Ecologico")] public float AddTime;

    [Header("UI Elements")] 
    public Image[] generatorStatusSprites;
    public Sprite activatedGenerator;
    public Sprite deactivatedGenerator;

    public Transform EnemyEmptyParent;

    [Header("Bocadillos")] 
    public Animator anim;
    public AnimatorOverrideController animatorBocadillo;

    #region Override Methods

    protected override void personalAwake()
    {
        base.personalAwake();
        enabledCloudList = new List<CloudAI>();
        spawnersDisablingList = new List<CloudSpawner>();
        enabledSpawners = new List<CloudSpawner>();
        Instance = this;
    }

    protected override void personalStart()
    {
        GameManager.Instance.playerScript.DisablePlayer();
        base.personalStart();
        _playerNet = FindObjectOfType<EquipableRedTest>();
        timer.PreSetTimmer();
    }

    protected override void OnGameStart()
    {
        GameManager.Instance.playerScript.sloopyMovement = true;
        base.OnGameStart();
        timer.SetTimer();
        TimeTryReference = Time.time;
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

    private void Update() {}

    #endregion

    #region OnEnable And OnDisable

    private void OnEnable()
    {
        ODS7Actions.OnCloudDelivered += CheckWinLose;
        ODS7Actions.OnFactoryDisabled += CheckWinLose;
        ODS7Actions.OnCloudDelivered += DeliverCloud;
        ODS7Actions.OnFactoryDisabled += PowerplantDeactivatedUI;
    }

    private void OnDisable()
    {
        ODS7Actions.OnCloudDelivered -= CheckWinLose;
        ODS7Actions.OnFactoryDisabled -= CheckWinLose;
        ODS7Actions.OnCloudDelivered -= DeliverCloud;
        ODS7Actions.OnFactoryDisabled -= PowerplantDeactivatedUI;
    }

    #endregion

    #region UI Functions

    public void PowerplantDeactivatedUI()
    {
        for (int i = 0; i < generatorStatusSprites.Length; i++)
        {
            if (generatorStatusSprites[i].sprite == activatedGenerator)
            {
                generatorStatusSprites[i].sprite = deactivatedGenerator;
                break;
            }
        }
    }

    #endregion

    #region Cloud Functions

    private void CloudRepairCheck()
    {
        if (Time.time - TimeTryReference < tryintervalFix)
            return;

        if (UnityEngine.Random.Range(0, ChanceBase) != 1 || spawnersDisablingList.Count == 0 ||
            enabledCloudList.Count == 0)
        {
            TimeTryReference = Time.time;
            return;
        }

        CloudSpawner targetPowerplant =
            spawnersDisablingList[UnityEngine.Random.Range(0, spawnersDisablingList.Count + 1)];
        CloudAI objectiveCloud = enabledCloudList[UnityEngine.Random.Range(0, enabledCloudList.Count + 1)];

        if (targetPowerplant.TargetAI != null || objectiveCloud.targetCloudSpawner != null)
            return;

        targetPowerplant.TargetAI = objectiveCloud;
        objectiveCloud.targetCloudSpawner = targetPowerplant;

        objectiveCloud.ReturnToBase(targetPowerplant.transform);
    }

    public void DisableCloud(CloudAI cloudToDisable)
    {
        if (!enabledCloudList.Contains(cloudToDisable)) return;
        enabledCloudList.Remove(cloudToDisable);
        disabledCloudList.Add(cloudToDisable);
    }

    public void DestroyCloud(CloudAI cloudToDestroy)
    {
        if (cloudToDestroy.isActiveAndEnabled)
        {
            cloudToDestroy.ResetMovement();
        }
        
        if (cloudToDestroy.targetCloudSpawner)
            cloudToDestroy.targetCloudSpawner.TargetAI = null;

        if (enabledCloudList.Contains(cloudToDestroy))
        {
            enabledCloudList.Remove(cloudToDestroy);
            Destroy(cloudToDestroy.gameObject);
        }
        else
        {
            disabledCloudList.Remove(cloudToDestroy);
            Destroy(cloudToDestroy.gameObject);
        }
    }

    public void CaptureCloud()
    {
        if (!_playerNet.isCloudCaptured) return;
        anim.runtimeAnimatorController = animatorBocadillo;
        anim.SetTrigger("On");
    }

    public void DeliverCloud()
    {
        anim.SetTrigger("Off");
        AddScore(1);
    }

    private CloudAI FindNearestActiveCloud(Vector3 factoryPosition)
    {
        CloudAI firstClosest = null;
        float closestDistSqr = Mathf.Infinity;

        foreach (CloudAI cloud in enabledCloudList)
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

    #region Powerplant Functions

    public void RequestReinforcements(CloudSpawner affectedSpawner)
    {
        if (affectedSpawner.TargetAI != null) return;
        
        CloudAI cloudCandidate = FindNearestActiveCloud(affectedSpawner.transform.position);
        
        if (cloudCandidate.targetCloudSpawner != null) return;
        
        affectedSpawner.TargetAI = cloudCandidate;
        cloudCandidate.targetCloudSpawner = affectedSpawner;
        affectedSpawner.TargetAI.ReturnToBase(affectedSpawner.transform);
    }

    #endregion
    
    #region Score Setting Functions

    public override void SetResult()
    {
        Debug.Log("Se hace tranquilo");
        Debug.Log(Score);

        RankImage.sprite = RankData.timerImageArray[MinigameData.CheckPointsState(Score)].sprite;

        if (Score == -1)
        {
            _ScoreText.Pretext = null;
            _ScoreText.Preset = null;
            if (enabledSpawners.Count > 0 && (enabledCloudList.Count + disabledCloudList.Count > 0)) 
            {
                _ScoreText.ChangeText("Power plants and clouds remaining");
            } 
            else if (enabledSpawners.Count > 0)
            {
                _ScoreText.ChangeText("You missed some power plants!");
            } 
            else if ((enabledCloudList.Count + disabledCloudList.Count) > 0)
            {
                _ScoreText.ChangeText("You didn't catch all the pollution!");
            }
        }
        else {
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
        if (enabledSpawners.Count > 0 || ((enabledCloudList.Count + disabledCloudList.Count) > 0)) Score = -1;
        else Score = (int)timer.Value;

        SaveValue(Score);
    }

    private void CheckWinLose()
    {
        if (enabledSpawners.Count <= 0) _allGeneratorsDisabled = true;
        if (enabledCloudList.Count + disabledCloudList.Count <= 0) _allCloudsCaptured = true;
        if (_allGeneratorsDisabled && _allCloudsCaptured) OnGameFinish();
    }

    #endregion
}
