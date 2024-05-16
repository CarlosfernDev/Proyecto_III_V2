using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ODS2Singleton : MinigameParent
{
    public static ODS2Singleton Instance;

    public TimerMinigame timer;

    [Header("Granjas Valor")]
    public float AddTime;
    public float ReduceTime;
    public float[] WaterTime;

    public float SeedTimer;
    public float WaterMaxTimer;
    public float CollectingTimer;
    public float BirghtTimeLeft;

    public int ScoreWatering;
    public int ScoreCollecting;
    public int ScoreCollectingDone;

    public List<VegetalCollector> CollectorList;
    public List<Granjas> FarmOrderList;
    public int[] WhenActiveFarm;
    public Vector3[] SpeedBost;
    private int IndexFarm = 0;

    private int TotalVegetal = 0;

    private void OnEnable()
    {
        timer.OnTimerOver.AddListener(OnGameFinish);
    }

    private void OnDisable()
    {
        timer.OnTimerOver.RemoveListener(OnGameFinish);
    }

    protected override void personalStart()
    {
        base.personalStart();
        timer.PreSetTimmer();
    }

    protected override void personalAwake()
    {
        base.personalAwake();
        Instance = this;
    }

    protected override void OnGameStart()
    {
        base.OnGameStart();
        timer.SetTimer();
        FarmCheck();
    }

    public override void OnGameFinish()
    {
        base.OnGameFinish();
    }

    public void OnVegetalDone()
    {
        TotalVegetal++;
        FarmCheck();
    }

    public void FarmCheck()
    {
        if (IndexFarm >= WhenActiveFarm.Length)
            return;

        Debug.Log("Pasas1");

        if (WhenActiveFarm[IndexFarm] > TotalVegetal)
            return;

        Debug.Log("Pasas2");

        if(!Vector3.Equals(SpeedBost[IndexFarm], Vector3.zero)) 
            GameManager.Instance.playerScript.BoostVelocidadPermanente(SpeedBost[IndexFarm].x, SpeedBost[IndexFarm].y, SpeedBost[IndexFarm].z);

        FarmOrderList[IndexFarm].StarFarm();
        IndexFarm++;

        FarmCheck();
    }

    public void EnableAllGps()
    {
        foreach(VegetalCollector collector in CollectorList)
        {
            collector.EnableGps();
        }
    }

    public void DisableAllGps()
    {
        foreach (VegetalCollector collector in CollectorList)
        {
            collector.DisableGps();
        }
    }

    public override void SetResult()
    {
        RankImage.sprite = RankData.timerImageArray[MinigameData.CheckPointsState(Score)].sprite;

        _ScoreText.ChangeText("Score: " + Score.ToString());

        _txHighScore.text = "High: " + Mathf.Clamp(MinigameData.maxPoints, 0, MinigameData.maxPoints);
    }

}
