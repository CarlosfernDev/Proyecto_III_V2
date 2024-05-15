using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(TimerMinigame))]
public class ODS7Singleton : MinigameParent
{
    public Timer _txTime;

    public static ODS7Singleton Instance;

    public TimerMinigame timer;

    [Header("Fabricas Variables")]
    public List<CloudSpawner> spawnersDisablingList;
    public List<CloudSpawner> enabledSpawners;

    public float timeFabricaDestroy;
    public float[] timeCloudRestoration;
    public float timeCloudSpawn;
    public int maxClouds;
    public List<CloudAI> enabledCloudList;
    public List<CloudAI> disabledCloudList;
    public GameObject CloudPrefab;

    [Header("Ghost Try")]
    public float tryintervalFix;
    public int ChanceBase;
    private float TimeTryReference;

    [Header("Punto Ecologico")]
    public float AddTime;

    [Header("UI Elements")] 
    public Image[] powerplantStatusSprites;
    public Sprite activatedPowerplant;
    public Sprite deactivatedPowerplant;

    public Transform SpawnParent;

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
        timer.PreSetTimmer();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - TimeTryReference < tryintervalFix)
            return;

        if (UnityEngine.Random.Range(0, ChanceBase) != 1 || spawnersDisablingList.Count == 0 || enabledCloudList.Count == 0)
        {
            TimeTryReference = Time.time;
            return;
        }

        CloudSpawner targetPowerplant = spawnersDisablingList[UnityEngine.Random.Range(0, spawnersDisablingList.Count)];
        CloudAI objectiveCloud = enabledCloudList[UnityEngine.Random.Range(0, enabledCloudList.Count)];

        if (targetPowerplant.TargetAI != null || objectiveCloud.targetCloudSpawner != null)
            return;

        targetPowerplant.TargetAI = objectiveCloud;
        objectiveCloud.targetCloudSpawner = targetPowerplant;

        objectiveCloud.ReturnToPowerplant(targetPowerplant.transform);
    }

    public override void OnGameFinish()
    {
        timer.PauseTimer();
        GameManager.Instance.playerScript.DisablePlayer();
        GameManager.Instance.playerScript.enabled = false;
        base.OnGameFinish();
    }

    protected override void OnGameStart()
    {
        GameManager.Instance.playerScript.sloopyMovement = true;
        base.OnGameStart();
        timer.SetTimer();
        TimeTryReference = Time.time;
    }
    
    public void FactoryDeactivatedUI()
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

    public void DisableCloud(CloudAI cloudToDisable)
    {
        if (enabledCloudList.Contains(cloudToDisable))
        {
            enabledCloudList.Remove(cloudToDisable);
            disabledCloudList.Add(cloudToDisable);
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
        SaveValue(timer.GetRealTime());
    }

}
