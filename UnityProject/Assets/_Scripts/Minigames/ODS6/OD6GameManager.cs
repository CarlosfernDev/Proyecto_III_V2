using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OD6GameManager : MonoBehaviour
{

    private Level LevelRef; 
    public static OD6GameManager Instance;
    public int Score;
    public TileNavigator TN;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        LevelRef = 0;
    }
    void Start()
    {
        LoadLevel1();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel1()
    {
        StartCoroutine(WairForNextLevel1());
    }
    public void LoadLevel2()
    {
        StartCoroutine(WairForNextLevel2());

    }
    public void LoadLevel3()
    {
        StartCoroutine(WairForNextLevel3());


    }

    IEnumerator WairForNextLevel1()
    {
        yield return new WaitForSeconds(4);
        PipeGrid.Instance.LoadLevel(1);
        LevelRef = Level.Level1;
        TN.moveToZeroZero();
    }

    IEnumerator WairForNextLevel2()
    {
        yield return new WaitForSeconds(4);
        PipeGrid.Instance.LoadLevel(2);
        LevelRef = Level.Level2;
        TN.moveToZeroZero();
    }

    IEnumerator WairForNextLevel3()
    {
        yield return new WaitForSeconds(4);
        PipeGrid.Instance.LoadLevel(3);
        LevelRef = Level.Level3;
        TN.moveToZeroZero();
    }

    public void checkConditions()
    {
        Debug.Log("puntCHECK: "+Score);
        
        if (LevelRef == Level.Level1)
        {
            Score = 0;
            LoadLevel2();
            
        }
        if (LevelRef == Level.Level2 && Score == 2)
        {
            Score = 0;
            LoadLevel3();
        }
        if (LevelRef == Level.Level3 && Score == 3)
        {
            ODS6GameManagerHeredado.instance.youWin = true;
            ODS6GameManagerHeredado.instance.OnGameFinish();
            //LoadLevel1();
            //LLAMAR A FINALIZAR GAME
            Debug.Log("HAS GANADO");
        }
    }
    public void ResetSelector()
    {

    }
}
public enum Level
{
    Level1,
    Level2,
    Level3
}
