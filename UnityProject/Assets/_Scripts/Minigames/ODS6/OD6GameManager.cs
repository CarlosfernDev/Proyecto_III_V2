using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OD6GameManager : MonoBehaviour
{

    private Level LevelRef; 
    public static OD6GameManager Instance;
    public int Score;

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
        PipeGrid.Instance.LoadLevel(1);
        LevelRef = Level.Level1;
    }
    public void LoadLevel2()
    {
        PipeGrid.Instance.LoadLevel(2);
        LevelRef = Level.Level2;
    }
    public void LoadLevel3()
    {
        PipeGrid.Instance.LoadLevel(3); 
        LevelRef = Level.Level3;

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
            LoadLevel1();
            //LLAMAR A FINALIZAR GAME
            Debug.Log("HAS GANADO");
        }
    }
}
public enum Level
{
    Level1,
    Level2,
    Level3
}
