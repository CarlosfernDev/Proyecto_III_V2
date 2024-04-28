using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigameData", menuName = "ScriptableObjects/MinigamesData", order = 1)]
public class MinigamesScriptableObjectScript : ScriptableObject
{
    public int ID;
    public int maxPoints;

    public int sceneID;

    [HideInInspector] public bool isDone = false;

    // Implementacion por cosmeticos falta

    #region WinneableState

    public void FinishCheckScore(int Score)
    {

        if (Score < maxPoints)
            return;

        maxPoints = Score;
        SaveManager.SaveMinigameData(ID - 1);
    }

    #endregion


    #region SaveLogic
    public void SetPuzzleOnLoad(SaveMinigame Value)
    {
        if(ID != Value.ID)
        {
            Debug.LogError("El archivo de guardado es corrupto en la parte de los minijuegos");
            return;
        }

        maxPoints = Value.MaxPoints;
        
    }

    public SaveMinigame GetSavePuzzle()
    {
        SaveMinigame temporalSave = new SaveMinigame();

        temporalSave.ID = ID;
        temporalSave.MaxPoints = maxPoints;

        return temporalSave;
    }

    #endregion
}
