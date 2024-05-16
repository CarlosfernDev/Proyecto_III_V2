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
    public int maxClouds;

    [Header("Factory Disabled")]
    public float tryintervalFix;
    public int ChanceBase;
    private float TimeTryReference;

    [Header("Punto Ecologico")] public float AddTime;

    [Header("UI Elements")] public Image[] powerplantStatusSprites;
    public Sprite activatedPowerplant;
    public Sprite deactivatedPowerplant;

    public Transform SpawnParent;

    [Header("Bocadillos")] public Animator anim;
    public AnimatorOverrideController animatorBocadillo;

    #region Override Functions

    protected override void personalAwake()
    {
        enabledCloudList = new List<CloudAI>();
        spawnersDisablingList = new List<CloudSpawner>();
        enabledSpawners = new List<CloudSpawner>();
        Instance = this;
        base.personalAwake();
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

    #region Main Functions

    void Update()
    {

    }

    #endregion

    #region UI Functions

    public void PowerplantDeactivatedUI()
    {
        for (int i = 0; i < powerplantStatusSprites.Length; i++)
        {
            if (powerplantStatusSprites[i].sprite == activatedPowerplant)
            {
                powerplantStatusSprites[i].sprite = deactivatedPowerplant;
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
        CaptureCloud();
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

    public CloudAI FindNearestActiveCloud(Vector3 factoryPosition)
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

        _ScoreText.ChangeText(Score);

        int minutos = Mathf.FloorToInt(MinigameData.maxPoints / 60);
        int segundos = Mathf.FloorToInt(MinigameData.maxPoints % 60);

        _txHighScore.text = "High: " + string.Format("{0:00}:{1:00}", minutos, segundos/*, milisegundos*/);
    }

    public override void SaveValue()
    {
        Score = timer.GetRealTime();
        SaveValue(Score);
    }

    #endregion
}
