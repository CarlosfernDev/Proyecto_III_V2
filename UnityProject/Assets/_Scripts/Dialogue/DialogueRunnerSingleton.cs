using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(DialogueRunner))]
public class DialogueRunnerSingleton : MonoBehaviour
{
    public static DialogueRunnerSingleton Instance;
    public DialogueRunner _dialogueRunner;
    public LineViewCustome _LineView;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
