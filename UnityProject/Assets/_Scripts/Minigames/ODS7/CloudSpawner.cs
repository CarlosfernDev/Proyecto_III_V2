using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CloudSpawner : LInteractableParent
{
    public enum factoryState {Wait, Spawning, Replacing, Disable}
    public factoryState myFactoryState = factoryState.Wait;
    
    [SerializeField] private Transform _spawnTransform;

    [SerializeField] private Slider SpawnSlider;
    [SerializeField] private Slider FixSlider;
    
    [SerializeField] private float minSpawnRadius = 1f;
    [SerializeField] private float maxSpawnRadius = 5f;

    [SerializeField] private int maxRepairLevels = 3;
    [SerializeField] private int _currentRepairLevel = 0;
    
    public CentralVFX CentralVFX;
    public CloudAI TargetAI;

    private bool _IsRecalculateTime;

    private float _TimeReferenceDestroy;
    private float _TimeReferenceSpawn;
    private float _SpawnTimeOffset = 0;
    private float _maxTimeToTransform;
    private float _currentTimeToTransform;

    private Vector3 _randomSpawnPoint;
    private bool _isSpawnPointSet;

    public float timeMultiplier = 1.0f;

    private void Start()
    {
        ODS7Singleton.Instance.enabledSpawners.Add(this);

        FixSlider.gameObject.SetActive(false);
        FixSlider.maxValue = ODS7Singleton.Instance.timeFabricaDestroy;
        FixSlider.value = 0;

        SpawnSlider.maxValue = ODS7Singleton.Instance.timeCloudSpawn;
        SpawnSlider.value = ODS7Singleton.Instance.timeCloudSpawn;

        ODS7Singleton.Instance.OnGameStartEvent += OnGameStart;
        _maxTimeToTransform = ODS7Singleton.Instance.timeFabricaDestroy;
        _currentTimeToTransform = _maxTimeToTransform;
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

        if (myFactoryState != factoryState.Replacing)
            return;

        DisabledState();
    }

    void DisabledState()
    {
        if (!FixSlider.gameObject.activeSelf)
        {
            SpawnSlider.gameObject.SetActive(false);
            FixSlider.gameObject.SetActive(true);
        }
        
        if (_currentRepairLevel >= maxRepairLevels)
        {
            RestoreFactory();
            _currentRepairLevel = 0;
            return; 
        } 

        if (TargetAI == null && ODS7Singleton.Instance.enabledCloudList.Count > 0)
        {
            ODS7Singleton.Instance.RequestReinforcements(this);
        }
        
        _currentTimeToTransform -= timeMultiplier * Time.deltaTime;
        float TimeLoad = _currentTimeToTransform;
        TimeLoad = Mathf.Clamp(TimeLoad, 0, ODS7Singleton.Instance.timeFabricaDestroy);

        FixSlider.value = TimeLoad;

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

        if (ODS7Singleton.Instance.maxClouds <= ODS7Singleton.Instance.enabledCloudList.Count)
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

        ODS7Singleton.Instance.enabledCloudList.Add(Cloud.GetComponent<CloudAI>());
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
        _SpawnTimeOffset = Time.time - _TimeReferenceSpawn;
        myFactoryState = factoryState.Disable;
        ODS7Singleton.Instance.enabledSpawners.Remove(this);
        if (TargetAI != null)
        {
            TargetAI.CancelReturn();
            TargetAI = null;
        }
        ODS7Singleton.Instance.AddScore(100);
        CentralVFX.CallCoroutine();
        ODS7Singleton.Instance.PowerplantDeactivatedUI();
        if (ODS7Singleton.Instance.enabledSpawners.Count <= 0) ODS7Singleton.Instance.OnGameFinish();
    }

    public void RestoreFactory()
    {
        FixSlider.gameObject.SetActive(false);
        SpawnSlider.gameObject.SetActive(true);
        ODS7Singleton.Instance.spawnersDisablingList.Remove(this);
        if (TargetAI != null)
        {
            TargetAI.CancelReturn();
            TargetAI = null;
        }
        
        _TimeReferenceSpawn = Time.time - _SpawnTimeOffset;
        myFactoryState = factoryState.Spawning;
    }

    // Cambiar el sistema respetando la bool de isInterecteable
    public override void Interact()
    {
        base.Interact();

        if (myFactoryState != factoryState.Spawning)
            return;

        myFactoryState = factoryState.Replacing;
        _TimeReferenceDestroy = Time.time;
        ODS7Singleton.Instance.spawnersDisablingList.Add(this);
        ODS7Singleton.Instance.RequestReinforcements(this);
    }

    private void SetSpawnLocation()
    {
        if (_isSpawnPointSet) return;
        Vector3 calculatedSpawnPoint = RandomPointInRing(_spawnTransform, new Vector2(_spawnTransform.position.x, _spawnTransform.position.z), minSpawnRadius, maxSpawnRadius);
        if (CanSpawnHere(calculatedSpawnPoint))
        {
            _randomSpawnPoint = calculatedSpawnPoint;
            _isSpawnPointSet = true;
        }
        else
        {
            _isSpawnPointSet = false;
        }
    }

    private bool CanSpawnHere(Vector3 spawnPoint)
    {
        Physics.Raycast(spawnPoint, Vector3.down, out RaycastHit rayHit, 10f);

        if (rayHit.collider.CompareTag("Ground"))
            return true;
        else
            return false;
    }

    private Vector3 RandomPointInRing(Transform spawnTransform, Vector2 origin, float minRadius, float maxRadius) 
    {
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
        float minRadius2 = minRadius * minRadius;
        float maxRadius2 = maxRadius * maxRadius;
        float randomDistance = Mathf.Sqrt(UnityEngine.Random.value * (maxRadius2 - minRadius2) + minRadius2);

        Vector2 randomPoint2D = (origin + randomDirection * randomDistance);
        
        return new Vector3(randomPoint2D.x, spawnTransform.position.y, randomPoint2D.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (myFactoryState != factoryState.Replacing) return;
        if (!other.TryGetComponent(out CloudAI cloudAI) && !cloudAI.isCaptured) return;
        ODS7Singleton.Instance.DestroyCloud(cloudAI);
        _currentRepairLevel++;
        timeMultiplier = timeMultiplier - 0.25f;
    }

#if (UNITY_EDITOR) 
    private void OnDrawGizmos()
    {
        var position = _spawnTransform.position;
        Handles.DrawWireDisc(position, Vector3.up, maxSpawnRadius);
        Handles.DrawWireDisc(position, Vector3.up, minSpawnRadius);
    }
#endif
}
