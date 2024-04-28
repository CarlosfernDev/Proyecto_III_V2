using IngameDebugConsole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvents : MonoBehaviour
{
    public void DialogueStartedEvent()
    {
        Debug.Log("Dialogue Started");
    }

    public void DialogueEndedEvent()
    {
        Debug.Log("Dialogue Ended");
    }
}
