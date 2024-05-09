using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Variables condicionales
    public bool isDialogueActive = false;
    [SerializeField] public MinigamesScriptableObjectScript[] MinigameScripts;
    [SerializeField] public PancartaScriptableObject[] PancartaData;

    [Header("PauseUI")]
    [SerializeField] public Canvas PauseCanvas;
    [SerializeField] public Canvas SettingCanvas;
    [SerializeField] public GameObject PauseUI;
    [SerializeField] public Button FirstButton;
    [SerializeField] private Button RestartButton;
    [SerializeField] public EventSystem eventSystem;
    [SerializeField] public SettingsMenu settingsScript;

    // Variables del jugador
    [HideInInspector] public TestInputs playerScript;
    [HideInInspector] public string playerName = "Fred";
    [HideInInspector] public int playerCoins = 0;

    [HideInInspector] public bool isPaused = false;
    [HideInInspector] public bool isPlaying = false;

    [HideInInspector] public enum ProgramState { Menu, Hub, Minigame }
    [HideInInspector] public ProgramState programState = ProgramState.Menu;

    [HideInInspector] public enum GameState { Aire, Puentes, Reciclaje, Mar, AguaLimpia, GranjaPlantas, GranjaZoo, Pancarta, PostGame }
    [HideInInspector] public GameState state = GameState.Aire;

    // GamesUnlocked
    //public Dictionary<int, Scriptable> minigamesScriptableDictionary; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InputManager.Instance.pauseEvent.AddListener(SetPause);

        PauseUI.SetActive(false);

        FirstTime();
        NextState(8);
    }

    private void Update()
    {

    }

    #region CalleableFunctions

    // Intentar no utilizar, por que puede generar muchos conflictos
    public void NextState()
    {
        state++;
        SaveManager.SavePlayerData();
    }

    /* 0 - Aire
     * 1 - Puentes
     * 2 - Reciclajes
     * 3 - Mar
     * 4 - AguaLimpia
     * 5 - GranjaPlantas
     * 6 - GranjaZoo
     * 7 - Pancarta
     * 8 - PostGame
     */
    public void NextState(int value)
    {
        state = (GameState)Mathf.Clamp(value, 0, 8);
        SaveManager.SavePlayerData();
    }

    public void SetCamera(Camera Component)
    {
        PauseCanvas.worldCamera = Component;
        PauseCanvas.planeDistance = -20f;

        SettingCanvas.worldCamera = Component;
        SettingCanvas.planeDistance = -20f;
    }

    #endregion

    #region Private Functions

    private void FirstTime()
    {
        if (Application.isEditor)
            return;

        if (!SaveManager.IsDirectoryExist())
        {
            SaveManager.ResetGame();
        }
        else
        {
            SaveManager.LoadSaveFileSetUp();
        }
    }

    #endregion

    #region PauseMenu

    public void SetPause(bool value)
    {
        if (!isPlaying)
            return;

        isPaused = value;

        CheckPause();
    }

    public void SetPause()
    {
        if (!isPlaying)
            return;

        isPaused = !isPaused;

        CheckPause();
    }

    public void CheckPause()
    {
        Debug.Log("Pausaste");

        PauseUI.SetActive(isPaused);
        if (isPaused)
        {
            Time.timeScale = 0;

            RestartButton.gameObject.SetActive(programState == ProgramState.Minigame);

            eventSystem.SetSelectedGameObject(FirstButton.gameObject);
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void OpenSettings()
    {
        settingsScript.EnableMenu();
        Time.timeScale = 0;
        isPlaying = false;
        PauseUI.SetActive(false);
    }

    public void RestartGame()
    {
        SetPause(false);
        MySceneManager.Instance.RestartScene();
    }

    public void CloseGame()
    {
        if (MySceneManager.ActualScene >= 10 && MySceneManager.ActualScene < 100)
        {
            SetPause(false);
            MySceneManager.Instance.NextScene(100,1,1,1);
        }
        else
        {
            Application.Quit();
        }
    }

    #endregion

    #region Setting Variables

    public void SetisDialogueActive(bool value)
    {
        Instance.isDialogueActive = value;
        Debug.Log("Dialogue Set To " + isDialogueActive);
    }
    #endregion
}
