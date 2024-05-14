using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigameData", menuName = "ScriptableObjects/MinigamesData", order = 1)]
public class MinigamesScriptableObjectScript : ScriptableObject
{
    public int ID;
    public int maxPoints = -1;

    public int sceneID;

    public List<int> PointsValue;

    [HideInInspector] public bool isDone = false;

    // Implementacion por cosmeticos falta

    #region WinneableState

    public int CheckPointsState(int Score)
    {
        if (Score < PointsValue[0]) return 0;
        if (Score < PointsValue[1]) return 1;
        return 2;
    }

    public void FinishCheckScore(int Score)
    {
        if (Score < maxPoints)
            return;

        if (GameManager.Instance != null) GameManager.Instance.UpdateStars();

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
