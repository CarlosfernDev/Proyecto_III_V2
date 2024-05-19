using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OD6GameManager : MonoBehaviour
{

    private Level LevelRef; 
    public static OD6GameManager Instance;
    public int Score;
    public TileNavigator TN;
    [SerializeField] TimerMinigame TM;

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

    public void LoadLevel4()
    {
        StartCoroutine(WairForNextLevel4());
    }

    public void LoadLevel5()
    {
        StartCoroutine(WairForNextLevel5());
    }

    IEnumerator WairForNextLevel1()
    {
        
        yield return new WaitForSeconds(5.5f);
        PipeGrid.Instance.LoadLevel(1);
        LevelRef = Level.Level1;
        TN.ResetPosZeroZero();
    }

    IEnumerator WairForNextLevel2()
    {
        
        AlexCameraFade.Instance.FadeOut();
        yield return new WaitForSeconds(1.5f);
        AlexCameraFade.Instance.FadeIn();
        PipeGrid.Instance.LoadLevel(2);
        LevelRef = Level.Level2;
        TN.ResetPosZeroZero();
    }

    IEnumerator WairForNextLevel3()
    {
        AlexCameraFade.Instance.FadeOut();
        yield return new WaitForSeconds(1.5f);
        AlexCameraFade.Instance.FadeIn();
        PipeGrid.Instance.LoadLevel(3);
        LevelRef = Level.Level3;
        TN.ResetPosZeroZero();
    }

    IEnumerator WairForNextLevel4()
    {
        AlexCameraFade.Instance.FadeOut();
        yield return new WaitForSeconds(1.5f);
        AlexCameraFade.Instance.FadeIn();
        PipeGrid.Instance.LoadLevel(4);
        LevelRef = Level.Level4;
        TN.ResetPosZeroZero();
    }

    IEnumerator WairForNextLevel5()
    {
        AlexCameraFade.Instance.FadeOut();
        yield return new WaitForSeconds(1.5f);
        AlexCameraFade.Instance.FadeIn();
        PipeGrid.Instance.LoadLevel(5);
        LevelRef = Level.Level5;
        TN.ResetPosZeroZero();
    }

    public void checkConditions()
    {
        Debug.Log("puntCHECK: "+Score);
        
        if (LevelRef == Level.Level1)
        {
            Score = 0;
            LoadLevel2();
            
        }
        if (LevelRef == Level.Level2 && Score == 1)
        {
            Score = 0;
            LoadLevel3();
        }
        if (LevelRef == Level.Level3 && Score == 2)
        {
            Score = 0;
            LoadLevel4();
           
        }
        if (LevelRef == Level.Level4 && Score == 5)
        {
            Score = 0;
            LoadLevel5();
            
        }
        if (LevelRef == Level.Level5 && Score == 6)
        {
            Score = 0;
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
    Level3,
    Level4,
    Level5
}
