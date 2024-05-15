using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CloudSpawner : LInteractableParent
{
    public enum factoryState {Wait, Spawning, Loading, Disable}
    public factoryState myFactoryState = factoryState.Wait;
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private Slider SpawnSlider;
    [SerializeField] private Slider FixSlider;
    [SerializeField] private float minSpawnRadius = 1f;
    [SerializeField] private float maxSpawnRadius = 5f;

    public CentralVFX CentralVFX;
    public CloudAI TargetAI;

    private bool _IsRecalculateTime;

    private float _TimeReferenceDestroy;
    
    private float _TimeReferenceSpawn;
    private float _SpawnTimeOffset = 0;

    private Vector3 _randomSpawnPoint;
    private bool _isSpawnPointSet;

    private void Start()
    {
        ODS7Singleton.Instance.enabledSpawners.Add(this);

        FixSlider.gameObject.SetActive(false);
        FixSlider.maxValue = ODS7Singleton.Instance.timeFabricaDestroy;
        FixSlider.value = 0;

        SpawnSlider.maxValue = ODS7Singleton.Instance.timeCloudSpawn;
        SpawnSlider.value = ODS7Singleton.Instance.timeCloudSpawn;

        ODS7Singleton.Instance.OnGameStartEvent += OnGameStart;
        _TimeReferenceSpawn = Time.time;
    }

    private void Update()
    {
        if (!ODS7Singleton.Instance.gameIsActive)   
            return;

        // Chequear si puede spawnear
        if (IsCloudSpawneable())
        {
            SetSpawnLocation();
            if (!_isSpawnPointSet) return;
            SpawnCloud();
        }
        // Si puede spawnea

        if (myFactoryState != factoryState.Loading)
            return;

        UpdateValue();
    }

    void UpdateValue()
    {
        if (!FixSlider.gameObject.activeSelf)
        {
            FixSlider.gameObject.SetActive(true);
        }

        float TimeLoad = ODS7Singleton.Instance.timeFabricaDestroy - (Time.time - _TimeReferenceDestroy);
        TimeLoad = Mathf.Clamp(TimeLoad, 0, ODS7Singleton.Instance.timeFabricaDestroy);

        FixSlider.value = TimeLoad;

        // Valorar suma de fantasmas

        /* if(TimeLoad == ODS7Singleton.Instance.timeFabricaDestroy)
        {
            RestoreFactory();
            return; 
        } */

        if (TimeLoad == 0)
        {
            FixSlider.gameObject.SetActive(false);
            DisableFactory();
            return;
        }
    }

    bool IsCloudSpawneable()
    {
        if (myFactoryState != factoryState.Spawning)
            return false;

        if (ODS7Singleton.Instance.maxClouds <= ODS7Singleton.Instance.cloudList.Count)
        {
            _TimeReferenceSpawn = Time.time;
            return false;
        }

        float TimeSpawn = ODS7Singleton.Instance.timeCloudSpawn - ((Time.time - _TimeReferenceSpawn));
        TimeSpawn = Mathf.Clamp(TimeSpawn, 0, ODS7Singleton.Instance.timeCloudSpawn);

        SpawnSlider.value = TimeSpawn;

        if (TimeSpawn == 0)
            return true;

        return false;
    }

    void SpawnCloud()
    {
        GameObject Cloud = Instantiate(ODS7Singleton.Instance.CloudPrefab, _randomSpawnPoint, Quaternion.identity);
        CentralVFX.SpawnCloudVFX();

        Cloud.transform.parent = ODS7Singleton.Instance.SpawnParent;

        ODS7Singleton.Instance.cloudList.Add(Cloud.GetComponent<CloudAI>());
        _TimeReferenceSpawn = Time.time;
        _SpawnTimeOffset = 0;
        _isSpawnPointSet = false;
    }

    void OnGameStart()
    {
        _TimeReferenceSpawn = Time.time;
        myFactoryState = factoryState.Spawning;
        ODS7Singleton.Instance.OnGameStartEvent -= OnGameStart;
    }

    void DisableFactory()
    {
        ODS7Singleton.Instance.AddScore(100);
        CentralVFX.CallCoroutine();
        _SpawnTimeOffset = Time.time - _TimeReferenceSpawn;
        myFactoryState = factoryState.Disable;
        ODS7Singleton.Instance.enabledSpawners.Remove(this);
        ODS7Singleton.Instance.FactoryDeactivatedUI();
        if (ODS7Singleton.Instance.enabledSpawners.Count <= 0) ODS7Singleton.Instance.OnGameFinish();
    }

    public void RestoreFactory()
    {
        FixSlider.gameObject.SetActive(false);
        ODS7Singleton.Instance.spawnersDisablingList.Remove(this);
        TargetAI.isReturningToPowerplant = false;
        TargetAI.targetCloudSpawner = null;
        TargetAI = null;
        _TimeReferenceSpawn = Time.time - _SpawnTimeOffset;
        myFactoryState = factoryState.Spawning;
    }

    // Cambiar el sistema respetando la bool de isInterecteable
    public override void Interact()
    {
        base.Interact();

        if (myFactoryState != factoryState.Spawning)
            return;

        myFactoryState = factoryState.Loading;
        _TimeReferenceDestroy = Time.time;
        ODS7Singleton.Instance.spawnersDisablingList.Add(this);
    }

    private void SetSpawnLocation()
    {
        if (_isSpawnPointSet) return;
        Vector3 _calculatedSpawnPoint = RandomPointInRing(spawnTransform, new Vector2(transform.position.x, transform.position.z), minSpawnRadius, maxSpawnRadius);
        if (CanSpawnHere(_calculatedSpawnPoint))
        {
            _randomSpawnPoint = _calculatedSpawnPoint;
            _isSpawnPointSet = true;
        }
        else
        {
            _isSpawnPointSet = false;
        }
    }

    private bool CanSpawnHere(Vector3 _spawnPoint)
    {
        Physics.Raycast(_spawnPoint, Vector3.down, out RaycastHit rayHit, 10f);

        if (rayHit.collider.CompareTag("Ground"))
            return true;
        else
            return false;
    }

    private Vector3 RandomPointInRing(Transform _spawnTransform, Vector2 origin, float minRadius, float maxRadius) 
    {
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
        float minRadius2 = minRadius * minRadius;
        float maxRadius2 = maxRadius * maxRadius;
        float randomDistance = Mathf.Sqrt(UnityEngine.Random.value * (maxRadius2 - minRadius2) + minRadius2);

        Vector2 randomPoint2D = (origin + randomDirection * randomDistance);
        
        return new Vector3(randomPoint2D.x, _spawnTransform.position.y, randomPoint2D.y);
    }

    /*private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, Vector3.up, maxSpawnRadius);
        Handles.DrawWireDisc(transform.position, Vector3.up,minSpawnRadius);
    }*/

}
