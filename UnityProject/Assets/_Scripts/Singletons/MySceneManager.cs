using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class MinigameUIData
{
    public Vector2 _PunteroVector;
    public Sprite _TutorialSprite;
    public string _Name;
}


public class MySceneManager : MonoBehaviour
{
    public static MySceneManager Instance;

    private Coroutine LoadCorutine;

    [SerializeField] private Animator _myanimator;
    [SerializeField] private List<AnimatorOverrideController> fadeTransition;
    [SerializeField] private float _tiempoMinimo;

    [Header("Panel Tutorial")]
    [SerializeField] private GameObject ingameCanvas;
    [SerializeField] private List<MinigameUIData> PunteroVector;
    [SerializeField] private RectTransform Puntero;
    [SerializeField] private TMP_Text TravelingText;
    [SerializeField] private RectTransform panelReference;
    [SerializeField] private Image _PanelImage;
    [SerializeField] private Animator _panelAnimator;

    private bool anyKeyIsPressed = false;

    public bool isLoading;
    public Action OnLoadFinish;
    public Dictionary<int, string> SceneDictionary;

    static public int ActualScene;

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

        _panelAnimator.gameObject.SetActive(false);

        LearnDictionary();
    }

    public void NextScene(int Value, int fadein, int fadeout, float MinimalTime)
    {
        if (isLoading) return;

        if (LoadCorutine != null)
        {
            StopCoroutine(LoadCorutine);
            LoadCorutine = null;
        }
        LoadCorutine = StartCoroutine(LoadCorutineFunction(Value, fadein, fadeout, MinimalTime));
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneDictionary[ActualScene]);
    }

    IEnumerator LoadCorutineFunction(int Value, int fadein, int fadeout, float LoadTime)
    {
        isLoading = true;
        if (InputManager.Instance != null) InputManager.Instance.interactEvent.RemoveAllListeners();
        if (InputManager.Instance != null) InputManager.Instance.movementEvent.RemoveAllListeners();
        if (InputManager.Instance != null) InputManager.Instance.equipableEvent.RemoveAllListeners();

        if (Value >= 10 && Value < 100)
        {
            int listvalue = Mathf.FloorToInt(Value / 10) - 1;
            ingameCanvas.SetActive(true);
            Puntero.anchoredPosition = PunteroVector[listvalue]._PunteroVector;
            _PanelImage.sprite = PunteroVector?[listvalue]._TutorialSprite;
            TravelingText.text = "Traveling to " + PunteroVector?[listvalue]._Name;
        }
        else
        {
            ingameCanvas.SetActive(false);
        }

        // Ejecuta la animacion de la transicion
        if(AudioManager.Instance != null) AudioManager.Instance.StartFade(1.2f, 0);
        _myanimator.runtimeAnimatorController = fadeTransition[fadein];
        _myanimator.SetTrigger("NextIn");

        // Comprueba cuando acaba la animacion
        while (true)
        {
            if (_myanimator.GetCurrentAnimatorStateInfo(0).IsName("1"))
                break;

            yield return null;
        }

        AsyncOperation loadLevel = null;

        if (SceneDictionary.ContainsKey(Value))
        {
            if (GameManager.Instance != null)
            {
                if (Value <= 9)
                    GameManager.Instance.programState = GameManager.ProgramState.Menu;
                else if (Value >= 10 && Value <= 79)
                    GameManager.Instance.programState = GameManager.ProgramState.Minigame;
                else if (Value == 100)
                {
                    GameManager.Instance.programState = GameManager.ProgramState.Hub;
                    Debug.Log((int)GameManager.Instance.state);

                    MinigamesScriptableObjectScript script = GameManager.Instance.MinigameScripts[GameManager.Instance.RelationGameStateMinigame[((int)GameManager.GameState.PostGame) - 1]];

                    if (!GameManager.Instance.PostGameEnabled && GameManager.Instance.state == ((GameManager.GameState.PostGame) - 1) && script.maxPoints >= script.PointsValue[0])
                    {
                        GameManager.Instance.PostGameEnabled = true;
                        GameManager.Instance.isPlaying = false;
                        GameManager.Instance.programState = GameManager.ProgramState.Menu;
                        Value = 102;
                        LoadTime = 0;
                    }
                }
                else if (Value == 101)
                    GameManager.Instance.programState = GameManager.ProgramState.Hub;
                else if (Value >= 102)
                {
                    GameManager.Instance.isPlaying = false;
                    GameManager.Instance.programState = GameManager.ProgramState.Menu;
                }
            }
            else
            {
                Debug.LogWarning("No hay GameManager");
            }

            ActualScene = Value;
            loadLevel = SceneManager.LoadSceneAsync(SceneDictionary[Value]);
        }
        else
            Debug.LogError("La escena no existe en el diccionario.");

        // Comprueba cuando ha acabado de cargar la pantalla (se puede poner mas tiempo si es necesario)
        float time = 0;
        while (true)
        {
            if (loadLevel.isDone && time >= LoadTime + _tiempoMinimo)
                break;

            time += Time.deltaTime;
            yield return null;
        }

        if (Value >= 10 && Value < 100)
        {
            _panelAnimator.gameObject.SetActive(true);

            _panelAnimator.SetTrigger("NextIn");

            while (true)
            {
                if (_panelAnimator.GetCurrentAnimatorStateInfo(0).IsName("1"))
                    break;
                yield return null;
            }

            InputManager.Instance.anyKeyEvent.AddListener(SetPressedButton);

            while (true)
            {
                if (anyKeyIsPressed)
                    break;

                yield return null;
            }

            InputManager.Instance.anyKeyEvent.RemoveListener(SetPressedButton);

            anyKeyIsPressed = false;

            _panelAnimator.SetTrigger("NextOut");
            while (true)
            {
                if (_panelAnimator.GetCurrentAnimatorStateInfo(0).IsName("0"))
                    break;
                yield return null;
            }

            _panelAnimator.gameObject.SetActive(false);
        }


        // Ejecuta la siguiente transicion
        if (AudioManager.Instance != null) AudioManager.Instance.SetMusic(Value);
        if (AudioManager.Instance != null) AudioManager.Instance.StartFade(1.2f, 1);
        _myanimator.runtimeAnimatorController = fadeTransition[fadeout];
        _myanimator.SetTrigger("NextOut");

        // Comprueba si la transicion se ha acabado
        while (true)
        {
            if (_myanimator.GetCurrentAnimatorStateInfo(0).IsName("0"))
                break;


            yield return null;
        }

        if (OnLoadFinish != null)
        {
            OnLoadFinish();
        }
        isLoading = false;
    }

    public void SetPressedButton()
    {
        anyKeyIsPressed = true;
    }

    private void ChargeScene(int Value)
    {
        if (SceneDictionary.ContainsKey(Value))
            SceneManager.LoadScene(SceneDictionary[Value]);
        else
            Debug.LogError("No existe esta entrada en la escena");
    }

    // Aqui es donde se asigna un numero al nombre de la escena, se debe hacer manualmente.
    // Intentaremos dejar del 1-9 para interfaces del menu.
    // Y del 10 al 80 para cada minijuego, donde 10 sera el primero el 20 el segundo...
    // El selector de niveles seran 100 y se le ira sumando uno por cada escena necesaria.
    private void LearnDictionary()
    {
        SceneDictionary = new Dictionary<int, string>();
        SceneDictionary.Add(1, "MainMenu");
        SceneDictionary.Add(2, "CUTSCENE");

        SceneDictionary.Add(10, "ODS2_FINAL");

        SceneDictionary.Add(20, "EscenaODSALEX");

        SceneDictionary.Add(30, "ODS7_FINAL");

        SceneDictionary.Add(40, "ODS9_FINAL");

        SceneDictionary.Add(50, "ODS10_FINAL");
        SceneDictionary.Add(51, "ODS10_FINAL");
        SceneDictionary.Add(52, "ODS10_FINAL");
        SceneDictionary.Add(53, "ODS10_FINAL");
        SceneDictionary.Add(54, "ODS10_FINAL");
        SceneDictionary.Add(55, "ODS10_FINAL");
        SceneDictionary.Add(56, "ODS10_FINAL");
        SceneDictionary.Add(57, "ODS10_FINAL");
        SceneDictionary.Add(58, "ODS10_FINAL");
        SceneDictionary.Add(59, "ODS10_FINAL");

        SceneDictionary.Add(60, "ODS12_FINAL");

        SceneDictionary.Add(70, "ODS14_FINAL");

        SceneDictionary.Add(80, "ODS_15");
        SceneDictionary.Add(81, "ODS_15_UISCENE");
        
        //SceneDictionary.Add(100, "HUBTESTEO");
        SceneDictionary.Add(100, "MAINCITY_FINAL");

        SceneDictionary.Add(101, "TUTORIAL_INTERACCION");
        SceneDictionary.Add(102, "ESCENA FINAL");
        SceneDictionary.Add(103, "Credits");

        //SceneDictionary.Add(20, "LevelSelector");
    }


}
