using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameManagerSergio : MinigameParent
{
    public static GameManagerSergio Instance;

    [SerializeField] private int numMateriales;

    [SerializeField] private TMP_Text uiMaterial;

    [SerializeField] public Vector3 actualSpawnPoint;

    [SerializeField] public int ScoreToSave;
    [SerializeField] public int ScoreUIFinger;

    [SerializeField] public TimerMinigame timer;
    [SerializeField] public TestInputs playerInputs;


    [SerializeField] public bool youWin = false;



    protected override void personalAwake()
    {
        base.personalAwake();
        Instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public void updateMaterial()
    {
        uiMaterial.text = numMateriales.ToString();
    }
    public void addMaterial(int _Material)
    {
        numMateriales += _Material;
        updateMaterial();
    }

    public void minusMaterial(int _Material)
    {
        numMateriales -= _Material;
        updateMaterial();
    }

    public int checkMaterial()
    {
        return numMateriales;
    }

    public void EnseñarPantallaFinal()
    {
        playerInputs.DisablePlayer();
        OnGameFinish();
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
        if (youWin)
        {
            SaveValue(timer.GetRealTime());
        }
        else
        {
            SaveValue(-1);
        }
        
    }
    protected override void OnGameStart()
    {
        base.OnGameStart();
        timer.SetTimer();
    }
}
