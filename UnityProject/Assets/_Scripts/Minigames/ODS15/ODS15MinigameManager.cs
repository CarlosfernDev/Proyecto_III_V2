using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ODS15MinigameManager : MinigameParent
{
    // Start is called before the first frame update
    public static ODS15MinigameManager instance;
    public int ScoreToSave;


    protected override void personalAwake()
    {
        base.personalAwake();
        instance = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void EnseñarPantallaFinal()
    {
        OnGameFinish();
    }

    public override void SetResult()
    {
        RankImage.sprite = RankData.timerImageArray[MinigameData.CheckPointsState(ScoreToSave)].sprite;

        _ScoreText.ChangeText("Score: " + ScoreToSave);

        _txHighScore.text = "High: " + MinigameData.maxPoints;
    }

    public override void SaveValue()
    {
        SaveValue(ScoreToSave);
    }


}
