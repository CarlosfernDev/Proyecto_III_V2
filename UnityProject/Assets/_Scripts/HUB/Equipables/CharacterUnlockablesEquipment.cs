using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class CharacterUnlockablesEquipment : MonoBehaviour
{
    [Header("Hat")]
    public List<GameObject> HatDefaultObject;
    public Transform HatPosition;
    public SkinnedMeshRenderer FaceMaterial;
    public Material FaceDefaultMaterial;
    [HideInInspector] public GameObject HatObject;

    [Header("Cape")]
    public Material CapeMaterial;

    [Header("Pet")]
    public Transform PetTransform;
    [HideInInspector] public GameObject PetObject;

    private void OnEnable()
    {
        UnlockablesManager.instance.ListCharacters.Add(this);


    }

    private void OnDisable()
    {
        UnlockablesManager.instance.ListCharacters.Remove(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            UnlockablesManager.instance.LoadHat(this, 2);
        }
    }
}
