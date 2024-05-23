using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockablesManager : MonoBehaviour
{
    public static UnlockablesManager instance;

    public List<HatScriptable> hatScripts;
    public List<CapeScriptable> capeScripts;
    public List<PetScriptable> petScripts;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
    }

    #region Cape

    #endregion

    #region Hat

    #endregion

    #region Pet

    #endregion
}
