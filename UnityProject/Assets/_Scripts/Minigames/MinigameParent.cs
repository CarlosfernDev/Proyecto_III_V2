using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class MinigameParent : MonoBehaviour
{
    [Header("Notes")]
    [SerializeField, Multiline(2)] private string notes;

    [Header("Variables")]

    public bool IsDeveloping;

    public Action OnGameStartEvent;

    [Header("Minigame Scriptable Object")]
    [SerializeField] protected MinigamesScriptableObjectScript MinigameData;
    [SerializeField] private Animator _uiAnimator;

    [Header("StartGameMinigame")]
    [SerializeField] private bool isCountdown;
    [SerializeField] private TimerScriptableObject TimerData;
    //[SerializeField] private TMP_Text _TextCanvas;
    [SerializeField] private Image CountRender;
    [SerializeField] private Animator NumberAnimation;
    private Coroutine _Coroutine;

    [Header("ScoreStats")]
    [SerializeField] private StatsButtons _statsButtons;
    [SerializeField] private Animator ResoultAnimator;
    [SerializeField] private GameObject ResultCanvas;
    [SerializeField] protected RankResoultScriptable RankData;
    [SerializeField] protected Image RankImage;
    [SerializeField] protected ScoreText _ScoreText;
    [SerializeField] protected TMP_Text _txHighScore;

    [HideInInspector] public int Score;
    public UnityEvent<int> OnScoreUpdate;
    private bool anyKeyIsPressed = false;

    [HideInInspector] public bool gameIsActive = false;

    private void Awake()
    {
        //if(_TextCanvas != null) _TextCanvas.gameObject.transform.parent.gameObject.SetActive(false);
        if (!IsDeveloping)
        {
            MySceneManager.Instance.OnLoadFinish += StartCountdown;
        }

        if (ResultCanvas != null)
        {
            ResultCanvas.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No existe el canvas de la pantalla final");
        }


        personalAwake();
    }

    private void Start()
    {
        // El timer debe llamarlo la pantalla de carga
        GameManager.Instance.isPlaying = true;

        personalStart();

        // Quitar ANTES DEL BUILD
        UpdateScore();
        if (IsDeveloping || !MySceneManager.Instance.isLoading)
        {
            StartCountdown();
        }
    }

    private void OnDestroy()
    {
        if (IsDeveloping) return;
        if (MySceneManager.Instance.OnLoadFinish != null && !gameIsActive)
            MySceneManager.Instance.OnLoadFinish -= StartCountdown;
    }

    private void StartCountdown()
    {
        if (CountRender == null) return;

        if(GameManager.Instance.playerScript != null) GameManager.Instance.playerScript.sloopyMovement = false;

        if (isCountdown)
        {
            Timer(3);
        }
        else
        {
            Timer(0);
        }

        if (IsDeveloping) return;

        if (MySceneManager.Instance.OnLoadFinish != null)
            MySceneManager.Instance.OnLoadFinish -= StartCountdown;
    }

    private void Timer(int value)
    {
        //_TextCanvas.gameObject.transform.parent.gameObject.SetActive(true);
        if (_Coroutine != null)
        {
            StopCoroutine(_Coroutine);
        }
        StartCoroutine(TimerCorutine(value));
    }
    private IEnumerator TimerCorutine(int value)
    {
        //_TextCanvas.gameObject.transform.parent.gameObject.SetActive(true);
        if (value != 0)
        {
            string text = "Time trial!";
            SetImage(4, TimerData);
            //_TextCanvas.text = text;

            yield return new WaitForSeconds(1.3f);
            while (true)
            {
                if (value == 0)
                {
                    SetImage(value, TimerData);
                    //if (_TextCanvas != null) text = "Go!";
                    OnGameStart();
                }
                else
                {
                    text = value.ToString();
                }

                //_TextCanvas.text = text;
                // Hacer animacion
                SetImage(value, TimerData);


                yield return new WaitForSeconds(1.3f);

                value--;
                if (value == -1)
                    break;
            }
        }
        else
        {
            SetImage(value, TimerData);
            string text = "Go!";
            OnGameStart();
            //_TextCanvas.text = text;


            yield return new WaitForSeconds(1);
        }
        //_TextCanvas.gameObject.transform.parent.gameObject.SetActive(false);
        gameIsActive = true;

        if (OnGameStartEvent != null)
            OnGameStartEvent();
    }

    protected void SetImage(int value, TimerScriptableObject timerData)
    {
        int valueData = Mathf.Clamp(value, 0, timerData.timerImageArray.Length - 1);
        CountRender.sprite = timerData.timerImageArray[valueData].sprite;
        //CountRender.SetNativeSize();
        CountRender.transform.localScale = timerData.timerImageArray[valueData].scale;
        NumberAnimation.SetTrigger(timerData.timerImageArray[valueData].AnimationInvoke);
    }

    protected virtual void personalAwake()
    {

    }

    protected virtual void personalStart()
    {

    }

    protected virtual void OnGameStart()
    {
        if (GameManager.Instance.playerScript != null) GameManager.Instance.playerScript.sloopyMovement = true;
    }

    public virtual void OnGameFinish()
    {
        gameIsActive = false;

        SaveValue();
        GameManager.Instance.isPlaying = false;

        _Coroutine = StartCoroutine(CoroutineOnGameFinish());
    }

    public virtual void SaveValue()
    {
        Debug.Log("Finished");
        try
        {
            MinigameData.FinishCheckScore(Score);
        }
        catch (Exception e)
        {
            Debug.LogWarning("No se ha podido guardar, probablemente te falta el SaveManager");
        }
    }

    public void SaveValue(int value)
    {
        Debug.Log("Finished");
        try
        {
            MinigameData.FinishCheckScore(value);
        }
        catch (Exception e)
        {
            Debug.LogWarning("No se ha podido guardar, probablemente te falta el SaveManager");
        }
    }

    IEnumerator CoroutineOnGameFinish()
    {
        //_TextCanvas.gameObject.transform.parent.gameObject.SetActive(true);
        SetImage(5, TimerData);

        yield return new WaitForSeconds(1.3f);

        InputManager.Instance.anyKeyEvent.AddListener(SetPressedButton);

        ResultCanvas.SetActive(true);
        SetResult();
        ResoultAnimator.SetTrigger("EnterAnimation");

        while (ResoultAnimator.GetCurrentAnimatorStateInfo(0).IsName("0"))
        {
            if (anyKeyIsPressed)
            {
                ResoultAnimator.speed = 3;
                break;
            }
            yield return null;
        }

        while (ResoultAnimator.GetCurrentAnimatorStateInfo(0).IsName("0"))
        {
            yield return null;
        }

        ResoultAnimator.speed = 1;
        InputManager.Instance.anyKeyEvent.RemoveListener(SetPressedButton);
        anyKeyIsPressed = false;

        _statsButtons.EnableButtons();

        // SceneManager hara cosas
    }

    public virtual void SetResult()
    {
        Debug.Log("Se hace tranquilo");
        Debug.Log(Score);

        RankImage.sprite = RankData.timerImageArray[MinigameData.CheckPointsState(Score)].sprite;

        _ScoreText.ChangeText(Score);
        _txHighScore.text = "High: " + MinigameData.maxPoints.ToString("000000");
    }

    public void SetPressedButton()
    {
        anyKeyIsPressed = true;
    }

    #region Score

    public void AddScore(int value)
    {
        Debug.Log(Score);
        Score = Score + value;
        UpdateScore();
    }

    public void RemoveScore(int value)
    {
        Score = Score - value;
        Score = Mathf.Clamp(Score, 0, 999999);
        UpdateScore();
    }

    public void UpdateScore()
    {
        if (OnScoreUpdate != null)
            OnScoreUpdate.Invoke(Score);
    }

    #endregion
}
