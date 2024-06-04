using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ManagerCutscene : MonoBehaviour
{
    public DialogueRunner DialogueRunner;
    public GameObject _button;

    public Animator _anim;
    public List<AnimatorOverrideController> AnimatorScene;

    public DialogueAdvanceInput InputsDialogue;

    public void EnableGame()
    {
        DialogueRunner.StartDialogue("Start");
        InputsDialogue.enabled = true;
        if (GameManager.Instance != null) GameManager.Instance.eventSystem.SetSelectedGameObject(_button);
        if(InputManager.Instance != null) InputManager.Instance.pauseEvent.AddListener(FinishScene);
        if (MySceneManager.Instance != null) MySceneManager.Instance.OnLoadFinish -= EnableGame;
    }

    public void Start()
    {
        if (MySceneManager.Instance != null) MySceneManager.Instance.OnLoadFinish += EnableGame;
        else EnableGame();

    }

    [YarnCommand("CutsceneChange")]
    public void CutsceneChange(int value)
    {
        _anim.runtimeAnimatorController = AnimatorScene[value-1];
    }

    [YarnCommand("CutsceneStop")]
    public void StopIddle(bool value)
    {
        _anim.SetBool("IsMoving", value);
        _anim.SetTrigger("UpdateIddle");
    }

    [YarnCommand("CutSceneJoin")]
    public void MoveAnim(bool value)
    {
        _anim.SetBool("IamJoining", value);
        _anim.SetTrigger("Move");
    }

    [YarnCommand("FinishCutscene")]
    public void FinishScene()
    {
        MySceneManager.Instance.NextScene(1, 1, 1, 1);
    }
}
