using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(DialogueRunner))]
public class DialogueRunnerSingleton : MonoBehaviour
{
    public static DialogueRunner Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = GetComponent<DialogueRunner>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
