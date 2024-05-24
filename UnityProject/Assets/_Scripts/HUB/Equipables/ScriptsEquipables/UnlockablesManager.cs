using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockablesManager : MonoBehaviour
{
    public static UnlockablesManager instance;

    public List<HatScriptable> hatScripts;
    public List<CapeScriptable> capeScripts;
    public List<PetScriptable> petScripts;

    public SaveItemsEquiped ItemSaved;

    public List<CharacterUnlockablesEquipment> ListCharacters;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;

        ItemSaved = new SaveItemsEquiped();
    }

    public void LoadFromSave(CharacterUnlockablesEquipment Character)
    {
        SaveItemsEquiped ItemsData = SaveManager.savePlayerData.PlayerItems;

        LoadHat(Character, ItemsData.Item1);
        LoadCape(Character, ItemsData.Item2);
        LoadPet(Character, ItemsData.Item3);
    }

    #region Cape
    public void SaveCape(int ID)
    {
        ItemSaved.Item2 = ID;
        SaveManager.SavePlayerData();

        foreach(CharacterUnlockablesEquipment character in ListCharacters)
        {
            LoadCape(character, ID);
        }
    }

    public void LoadCape(CharacterUnlockablesEquipment Character, int ID)
    {
        Character.CapeMaterial.SetTexture("_MainTex", capeScripts[ID].Cape);
    }
    #endregion

    #region Hat
    public void SaveHat(int ID)
    {
        ItemSaved.Item1 = ID;
        SaveManager.SavePlayerData();

        foreach (CharacterUnlockablesEquipment character in ListCharacters)
        {
            LoadHat(character, ID);
        }
    }

    public void LoadHat(CharacterUnlockablesEquipment Character, int ID)
    {
        HatScriptable scriptable = hatScripts[ID];

        foreach (GameObject HatPart in Character.HatDefaultObject)
        {
            HatPart.SetActive(!scriptable.DisableDefaultHat);
        }

        if (scriptable.HeadMaterial != null) Character.FaceMaterial.material = scriptable.HeadMaterial;
        else Character.FaceMaterial.material = Character.FaceDefaultMaterial;

        if(scriptable.HatPrefab != null) GameObject.Instantiate(scriptable.HatPrefab, Character.HatPosition);
    }
    #endregion

    #region Pet
    public void SavePet(int ID)
    {
        ItemSaved.Item3 = ID;
        SaveManager.SavePlayerData();

        foreach (CharacterUnlockablesEquipment character in ListCharacters)
        {
            LoadPet(character, ID);
        }
    }

    public void LoadPet(CharacterUnlockablesEquipment Character, int ID)
    {
        GameObject.Instantiate(petScripts[ID].PetPrefab, Character.PetTransform);
    }
    #endregion
}
