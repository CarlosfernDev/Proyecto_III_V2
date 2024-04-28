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

    public int ScoreWatering;
    public int ScoreCollecting;
    public int ScoreCollectingDone;

    public List<Granjas> FarmOrderList;
    public int[] WhenActiveOne;
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

    public void OnVegetalDone()
    {
        TotalVegetal++;
        FarmCheck();
    }

    public void FarmCheck()
    {
        if (IndexFarm >= WhenActiveOne.Length)
            return;

        Debug.Log("Pasas1");

        if (WhenActiveOne[IndexFarm] > TotalVegetal)
            return;

        Debug.Log("Pasas2");

        FarmOrderList[IndexFarm].StarFarm();

        IndexFarm++;

        FarmCheck();
    }

}
