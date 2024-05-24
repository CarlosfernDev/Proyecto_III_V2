using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SaveManager : MonoBehaviour
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";

    public static SaveData saveState = new SaveData();
    public static SavePlayerData savePlayerData = new SavePlayerData();

    public PancartaScriptableObject[] pancartaScriptableObjects;

    #region SaveThings

    private void Start()
    {
        foreach (PancartaScriptableObject pancarta in pancartaScriptableObjects) pancarta.LoadTexture();
    }

    public static void SavePlayerData()
    {
        savePlayerData.name = GameManager.Instance.playerName;
        savePlayerData.coins = GameManager.Instance.playerCoins;
        savePlayerData.PlayerItems = UnlockablesManager.instance.ItemSaved;
        savePlayerData.SaveState = GameManager.Instance.state;

        saveState.SavePlayerData = savePlayerData;
        saveState.Work = true;

        string Json = JsonUtility.ToJson(saveState);
        SaveSaveString(Json);
    }

    public static void SaveMinigameData(int value)
    {

        saveState.SaveMinigame[value] = GameManager.Instance.MinigameScripts[value].GetSavePuzzle(); 
        saveState.Work = true;

        string Json = JsonUtility.ToJson(saveState);
        SaveSaveString(Json);
    }

    public static void SaveAllMinigameData()
    {
        Debug.Log(GameManager.Instance.MinigameScripts.Length);
        for(int i = 0; i < GameManager.Instance.MinigameScripts.Length; i++)
        {
            saveState.SaveMinigame[i] = GameManager.Instance.MinigameScripts[i].GetSavePuzzle();
            Debug.Log(i);
        }
        saveState.Work = true;

        string Json = JsonUtility.ToJson(saveState);
        SaveSaveString(Json);
    }

    public static void SavePancarta(int value)
    {
        if (GameManager.Instance.PancartaData.Length - 1 >= value) return;

        PancartasData PancartaSaved = new PancartasData();

        PancartaSaved.ID = value;
        PancartaSaved.MaxPoints = GameManager.Instance.PancartaData[value].Score;

        saveState.SavePancarta[value] = PancartaSaved;
        saveState.Work = true;

        string Json = JsonUtility.ToJson(saveState);
        SaveSaveString(Json);
    }

    public static void SaveAllPancarta()
    {
        Debug.Log(GameManager.Instance.PancartaData.Length);
        for (int i = 0; i < GameManager.Instance.PancartaData.Length; i++)
        {
            PancartasData PancartaSaved = new PancartasData();

            PancartaSaved.ID = i;
            PancartaSaved.MaxPoints = GameManager.Instance.PancartaData[i].Score;

            saveState.SavePancarta[i] = PancartaSaved;
            saveState.Work = true;

            Debug.Log(i);
        }
        saveState.Work = true;

        string Json = JsonUtility.ToJson(saveState);
        SaveSaveString(Json);
    }

    public static void SaveAllUnlockable()
    {
        SaveUnlockable[] save = new SaveUnlockable[UnlockablesManager.instance.Notificator.Unlockables.Count];
        int idlenght = -1;
        int idLenghtUsable = 0;
        foreach (SUnlockable lockable in UnlockablesManager.instance.Notificator.Unlockables)
        {
            idlenght++;
            Debug.Log("Estoy revisando el logro: " + idlenght);
            if (!lockable.IsUnlocked) continue;

            SaveUnlockable newUnlockable = new SaveUnlockable();

            newUnlockable.ID = idlenght;

            save[idLenghtUsable] = newUnlockable;
            idLenghtUsable++;
        }
        Debug.Log("Termine de revisarlo todo");
        saveState.SaveUnlockable = save;

        saveState.Work = true;

        string Json = JsonUtility.ToJson(saveState);
        SaveSaveString(Json);
        Debug.Log("Lo escribi todo");
    }

    public static void ResetGame()
    {
        CreateDirectory();
        SaveAllMinigameData();
        SaveAllPancarta();
        SavePlayerData();
        LoadSaveFileSetUp();
    }

    public static void ResetAllGameInGame()
    {
        if (!Application.isEditor) CreateDirectory();

        foreach (PancartaScriptableObject pancarta in GameManager.Instance.PancartaData)
        {
            pancarta.Score = 0;
            pancarta.RemoveTexture();
        }

        foreach (MinigamesScriptableObjectScript minigame in GameManager.Instance.MinigameScripts)
        {
            minigame.maxPoints = -1;
        }

        foreach (SUnlockable lockable in UnlockablesManager.instance.Notificator.Unlockables)
        {
            lockable.IsUnlocked = false;
        }

        if (GameManager.Instance != null) Destroy(GameManager.Instance.gameObject);
        if (InputManager.Instance != null) Destroy(InputManager.Instance.gameObject);
        if (AudioManager.Instance != null) Destroy(AudioManager.Instance.gameObject);
        if (UnlockablesManager.instance != null) Destroy(UnlockablesManager.instance.gameObject);

        if (IsDirectoryExist())
        {
            Directory.Delete(SAVE_FOLDER, true);
            File.Delete(SAVE_FOLDER);
        }

        SceneManager.LoadScene("Preload");
        Time.timeScale = 1;
    }

    #endregion

    #region LoadThings

    public static void LoadSaveFile()
    {
        try
        {
            saveState = JsonUtility.FromJson<SaveData>(LoadSaveString());
            if (!saveState.Work)
                Debug.LogWarning("No se cargo los datos");

            savePlayerData = saveState.SavePlayerData;
        }
        catch {
            ResetGame();
            Debug.LogWarning("No se ha conseguido leer el archivo");
        }
    }

    public static void LoadSaveFileSetUp()
    {
        LoadSaveFile();

        GameManager.Instance.playerName = savePlayerData.name;
        GameManager.Instance.playerCoins = savePlayerData.coins;
        GameManager.Instance.state = savePlayerData.SaveState;
        UnlockablesManager.instance.ItemSaved = savePlayerData.PlayerItems;

        for (int i = 0; i > GameManager.Instance.MinigameScripts.Length; i++)
        {
            GameManager.Instance.MinigameScripts[i].SetPuzzleOnLoad(saveState.SaveMinigame[i]);
        }

        // Cargara items equipados

        // Foreach cargando minijuegos completados
        for (int i = 0; i < GameManager.Instance.MinigameScripts.Length; i++)
        {
            GameManager.Instance.MinigameScripts[i].SetPuzzleOnLoad(saveState.SaveMinigame[i]);
        }

        // Foreach cargando coleccionables desbloqueados

        for (int i = 0; i < GameManager.Instance.PancartaData.Length; i++)
        {
            GameManager.Instance.PancartaData[i].Score = saveState.SavePancarta[i].MaxPoints;
        }

        foreach (SaveUnlockable saveUnl in saveState.SaveUnlockable)
        {
            UnlockablesManager.instance.Notificator.Unlockables[saveUnl.ID].IsUnlocked = true;
        }
    }

    #endregion

    #region TextManager

    public static void SaveSaveString(string saveString)
    {
        if (Application.isEditor)
            return;

        File.WriteAllText(SAVE_FOLDER + "save.txt", saveString + Environment.NewLine + Environment.NewLine + "// PLZ, DO NOT HACK TEH GAME BY MODIFYIN DIS FILE. FINKZ DAT TEH GAME IZ INTENDD 2 BE DEVELOPR WIF TEH GOAL 2 EDUCATE AN NOT 2 BE CHALLENGE. :C");
    }

    public static string LoadSaveString()
    {
        if (Application.isEditor)
        {
            Debug.LogWarning("Se abrio el juego en editor, el save manager no funcionara.");
            return null;
        }

        if (System.IO.File.Exists(SAVE_FOLDER + "save.txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "/save.txt");
            saveString = saveString.Split(Environment.NewLine)[0];

            Debug.Log(saveString);
            return saveString;
        }
        else
        {
            Debug.LogWarning("No save");
            return null;
        }
    }

    public static bool IsDirectoryExist()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            return false;
        }
        return true;
    }

    public static bool IsSaveFileExist()
    {
        if(!System.IO.File.Exists(SAVE_FOLDER + "save.txt"))
        {
            return false;
        }
        return true;
    }

    public static void CreateDirectory()
    {
        if (Application.isEditor)
            return;

        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    #endregion
}

#region Class

[Serializable]
public class SavePlayerData {
    public string name = "Fred";
    public int coins = 0;
    public GameManager.GameState SaveState = GameManager.GameState.Energia;
    public SaveItemsEquiped PlayerItems = null;
}

[Serializable]
public class SaveItemsEquiped
{
    public int Item1;
    public int Item2;
    public int Item3;
}

[Serializable]
public class SaveUnlockable { 
    public int ID;
}

[Serializable]
public class SaveMinigame
{
    public int ID;
    public int MaxPoints;
}

[Serializable]
public class SaveData
{
    public bool Work = false;
    public SavePlayerData SavePlayerData = new SavePlayerData();
    public SaveMinigame[] SaveMinigame = new SaveMinigame[8];
    public SaveUnlockable[] SaveUnlockable;
    public PancartasData[] SavePancarta = new PancartasData[8];
}

[Serializable]
public class PancartasData
{
    public int ID;
    public int MaxPoints;
}

#endregion
