using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;

[RequireComponent(typeof(TimerMinigame))]
public class ODS7Singleton : MinigameParent
{
    public Timer _txTime;

    public static ODS7Singleton Instance;

    public TimerMinigame timer;

    [Header("Fabricas Variables")]
    public List<CloudSpawner> spawnersDisablingList;

    public float timeFabricaDestroy;
    public float[] timeCloudRestoration;
    public float timeCloudSpawn;
    public int maxClouds;
    public List<IAnube> cloudList;
    public GameObject CloudPrefab;
    public List<CloudSpawner> EnableFlats;

    [Header("Ghost Try")]
    public float tryintervalFix;
    public int ChanceBase;
    private float TimeTryReference;

    [Header("Punto Ecologico")]
    public float AddTime;

    public Transform SpawnParent;

    protected override void personalAwake()
    {
        cloudList = new List<IAnube>();
        spawnersDisablingList = new List<CloudSpawner>();
        EnableFlats = new List<CloudSpawner>();
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

        Debug.Log("Intento ciclo");

        if (UnityEngine.Random.Range(0, ChanceBase) != 1 || spawnersDisablingList.Count == 0 || cloudList.Count == 0)
        {
            TimeTryReference = Time.time;
            return;
        }

        Debug.Log("ChanceDada");

        CloudSpawner objectiveFabric = spawnersDisablingList[UnityEngine.Random.Range(0, spawnersDisablingList.Count)];
        IAnube objectiveCloud = cloudList[UnityEngine.Random.Range(0, cloudList.Count)];

        if (objectiveFabric.IAObjective != null || objectiveCloud.objectiveCloudSpawner != null)
            return;

        objectiveFabric.IAObjective = objectiveCloud;
        objectiveCloud.objectiveCloudSpawner = objectiveFabric;

        objectiveCloud.WEGOINGTOFACTORYYYYYYYYBOYSSS(objectiveFabric.transform);
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
