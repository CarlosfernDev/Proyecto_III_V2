using System.Collections;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GUIAreUSure areusure;

    public bool PostGameEnabled = false;

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

    public int[] RelationGameStateMinigame;
    [HideInInspector] public enum GameState { Puentes, Aire, Reciclaje, Mar, GranjaPlantas, GranjaZoo, Potibilizadora, PostGame }
    [HideInInspector] public GameState state = GameState.Puentes;

    public ScoreText StarsText;
    public Animator StarsAnimator;
    public float MinimalTimeToAfk;
    private float AfkTimeReference;
    private bool afkHudIsEnable;

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

        UpdateStars();
        FirstTime();
    }

    private void Update()
    {
        if (!isPlaying || Time.timeScale != 1 || programState == ProgramState.Menu || MySceneManager.Instance.isLoading == false) 
        {
            if (afkHudIsEnable) {
                afkHudIsEnable = false;
                StarsAnimator.SetTrigger("Reset");
            }
            AfkTimeReference = Time.time;
            return;
        }

        if (InputManager.Instance.playerInputs.ActionMap1.AnyKey.IsInProgress())
        {
            if (afkHudIsEnable) DisableAFKHUD();
            AfkTimeReference = Time.time;
        }
        else
            CheckAfk();
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
        Debug.Log(state);
    }

    public void UpdateState()
    {
        int nextState = 0;
        foreach(int value in RelationGameStateMinigame)
        {
            if (MinigameScripts[value].maxPoints == -1)
            {
                break;
            }

            if ((GameManager.GameState)(nextState + 1) == GameManager.GameState.PostGame)
            {
                nextState++;
                break;
            }

            nextState++;
        }

        NextState(nextState);
        Debug.Log(state);
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
        if (!isPlaying || MySceneManager.Instance.isLoading)
            return;

        isPaused = value;

        CheckPause();
    }

    public void SetPause()
    {
        if (!isPlaying || MySceneManager.Instance.isLoading)
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
            if (Time.timeScale != 0) StartCoroutine(WaitInputStop());
            else eventSystem.SetSelectedGameObject(FirstButton.gameObject);

            Time.timeScale = 0;
            RestartButton.gameObject.SetActive(programState == ProgramState.Minigame);
        }
        else
        {
            Time.timeScale = 1;
            eventSystem.SetSelectedGameObject(null);
        }
    }

    IEnumerator WaitInputStop()
    {
        while (InputManager.Instance.playerInputs.ActionMap1.Movement.ReadValue<Vector2>().y != 0f)
        {
            yield return new WaitForNextFrameUnit();
        }
        yield return new WaitForNextFrameUnit();

        if (isPaused) eventSystem.SetSelectedGameObject(FirstButton.gameObject);
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
            areusure.ChangeText("Do you want close the game?");
            InputManager.Instance.pauseEvent.RemoveListener(SetPause);
            areusure._FunctionOnYes += QuitGame;
            areusure._FunctionOnNo += restorePause;
            areusure.EnableMenu();
        }
    }

    public void restorePause()
    {
        InputManager.Instance.pauseEvent.AddListener(SetPause);
        CheckPause();
        areusure._FunctionOnNo -= restorePause;
    }

    public void QuitGame()
    {
        Application.Quit();
        areusure._FunctionOnYes -= QuitGame;
    }

    #endregion

    #region Setting Variables

    public void SetisDialogueActive(bool value)
    {
        Instance.isDialogueActive = value;
        if (Instance.playerScript != null) {
            Instance.playerScript.rb.velocity = Vector3.zero;
            Debug.Log("Hay script");
        }
        Debug.Log("Dialogue Set To " + isDialogueActive);
    }
    #endregion

    #region ScoreCanvas

    public void UpdateStars()
    {
        int StarsValue = 0;
        foreach (MinigamesScriptableObjectScript Minigame in MinigameScripts)
        {
            StarsValue += Minigame.CheckPointsState(Minigame.maxPoints);
        }
        StarsText.ChangeText(StarsValue);
    }

    public void CheckAfk()
    {
        if (afkHudIsEnable) return;

        if (MinimalTimeToAfk > Time.time - AfkTimeReference) return;

        EnableAFKHUD();
    }

    public void EnableAFKHUD()
    {
        if (afkHudIsEnable) return;
        afkHudIsEnable = true;
        StarsAnimator.SetBool( "IsEnable", afkHudIsEnable);
        StarsAnimator.SetTrigger("Move");
    }

    public void DisableAFKHUD()
    {
        if (!afkHudIsEnable) return;
        afkHudIsEnable = false;
        StarsAnimator.SetBool("IsEnable", afkHudIsEnable);
        StarsAnimator.SetTrigger("Move");
    }

    #endregion
}
