using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameManagerSergio : MinigameParent
{
    public static GameManagerSergio Instance;

    [SerializeField] private int numMateriales;

    [SerializeField] private TMP_Text uiMaterial;

    [SerializeField] public Vector3 actualSpawnPoint;

    void Start()
    {
        
    }

    protected override void personalAwake()
    {
        Instance = this;
        base.personalAwake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void updateMaterial()
    {
        uiMaterial.text = numMateriales.ToString();
    }
    public void addMaterial(int _Material)
    {
        numMateriales += _Material;
        updateMaterial();
    }

    public void minusMaterial(int _Material)
    {
        numMateriales -= _Material;
        updateMaterial();
    }

    public int checkMaterial()
    {
        return numMateriales;
    }
}
