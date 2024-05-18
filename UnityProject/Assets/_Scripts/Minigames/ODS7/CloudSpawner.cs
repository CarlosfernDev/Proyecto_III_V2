using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CloudSpawner : LInteractableParent
{
    public enum factoryState { Wait, Spawning, Transforming, Disable, Resetting }
    public factoryState myFactoryState = factoryState.Wait;
    
    [Header("Spawn Variables")]
    [SerializeField] private Transform _spawnTransform;
    [SerializeField] private float minSpawnRadius = 1f;
    [SerializeField] private float maxSpawnRadius = 5f;
    [SerializeField] private float _spawnOffset;
    [SerializeField] private float _offsetSpawnTime;

    [Header("UI Elements")]
    [SerializeField] private Slider SpawnSlider;
    [SerializeField] private Slider FixSlider;
    
    [Header("Transformation Stats")]
    [SerializeField] private int maxRepairLevels = 3;
    [SerializeField] private int _currentRepairLevel = 0;
    public GameObject[] repairLevelImage;
    public CloudAI TargetAI;
    public float timeMultiplier = 1.0f;
    
    [Header("VFX")]
    public CentralVFX VFXManager;
    public GameObject cloudSpawnVFX;

    private bool _IsRecalculateTime;

    private float _transformTimeRef;
    private float _spawnTimeRef;
    private float _nextSummonTimeRef;

    private float _maxNextSummonTime;
    private float _maxSpawnTime;
    private float _maxTransformTime;
    
    [Header("Debug")]
    [SerializeField] private float _currentSpawnTime;
    [SerializeField] private float _currentTransformTime;
    [SerializeField] private float _currentNextSummonTime;

    private Vector3 _randomSpawnPoint;
    private bool _isSpawnPointSet;


    #region Awake, Start, Update, OnGameStart Methods

    private void Start()
    {
        ODS7Singleton.Instance.enabledSpawners.Add(this);

        FixSlider.gameObject.SetActive(false);
        FixSlider.maxValue = ODS7Singleton.Instance.timeFabricaDestroy;
        FixSlider.value = 0;

        SpawnSlider.maxValue = ODS7Singleton.Instance.timeCloudSpawn;
        SpawnSlider.value = SpawnSlider.maxValue;
        
        ResetWrenches();

        ODS7Singleton.Instance.OnGameStartEvent += OnGameStart;
        _maxTransformTime = ODS7Singleton.Instance.timeFabricaDestroy;
        _maxSpawnTime = ODS7Singleton.Instance.timeCloudSpawn;
        _maxNextSummonTime = ODS7Singleton.Instance.timeToNewCloudCall;
        _currentTransformTime = 0;
    }
    
    private void Update()
    {
        if (!ODS7Singleton.Instance.gameIsActive)   
            return;

        switch (myFactoryState)
        {
            case factoryState.Spawning:
                SpawningState();
                break;
            case factoryState.Transforming:
                TransformingState();
                break;
            case factoryState.Resetting:
                ResettingState();
                break;
        }
    }

    void OnGameStart()
    {
        myFactoryState = factoryState.Spawning;
        _spawnTimeRef = Time.time;
        ODS7Singleton.Instance.OnGameStartEvent -= OnGameStart;
        _spawnOffset = RandomRoundOffset();
    }
    
    #endregion

    #region OnEnable And OnDisable

    private void OnEnable()
    {
        ODS7Actions.OnFactoryDisabled += ResetWrenches;
    }

    private void OnDisable()
    {
        ODS7Actions.OnFactoryDisabled -= ResetWrenches;
    }

    #endregion

    #region Generator States

    void SpawningState()
    {
        if (IsCloudSpawneable()) // Checks if spawning is possible
        {
            SetSpawnLocation();
            if (!_isSpawnPointSet) return;
            SpawnCloud();
        }
    }

    void TransformingState()
    {
        if (!FixSlider.gameObject.activeSelf)
        {
            SpawnSlider.gameObject.SetActive(false);
            FixSlider.gameObject.SetActive(true);
        }
        
        if (_currentRepairLevel >= maxRepairLevels)
        {
            myFactoryState = factoryState.Resetting;
            _currentRepairLevel = 0;
            return; 
        } 

        if (TargetAI == null && ODS7Singleton.Instance.activeClouds.Count > 0)
        {
            ODS7Singleton.Instance.RequestReinforcements(this);
            _nextSummonTimeRef = 0;
        }

        if (TargetAI != null && TargetAI.isCaptured)
        {
            if (_nextSummonTimeRef == 0)
            {
                _nextSummonTimeRef = Time.time;
            }
            CallNewCloud();
        }
        
        _currentTransformTime = timeMultiplier * (Time.time - _transformTimeRef);
        _currentTransformTime = Mathf.Clamp(_currentTransformTime, 0, _maxTransformTime);

        FixSlider.value = _currentTransformTime;

        if (_currentTransformTime >= _maxTransformTime)
        {
            FixSlider.gameObject.SetActive(false);
            DisableFactory();
            return;
        }
    }

    void ResettingState()
    {
        float value = _currentTransformTime - (4f*(Time.time - _transformTimeRef));
        FixSlider.value = value;
        
        if (value > 0) return;
        RestoreFactory();
    }

    #endregion

    #region Cloud Spawn Methods

    bool IsCloudSpawneable()
    {
        if (myFactoryState != factoryState.Spawning)
            return false;

        if (ODS7Singleton.Instance.maxClouds <= ODS7Singleton.Instance.activeClouds.Count)
        {
            _currentSpawnTime = 0;
            _spawnTimeRef = Time.time;
            _spawnOffset = RandomRoundOffset();
            SpawnSlider.gameObject.SetActive(false);
            return false;
        }
        if (!SpawnSlider.gameObject.activeSelf) SpawnSlider.gameObject.SetActive(true);

        _currentSpawnTime = (Time.time - _spawnTimeRef);
        _currentSpawnTime = Mathf.Clamp(_currentSpawnTime, 0, _offsetSpawnTime);
        SpawnSlider.value = _currentSpawnTime;
        
        if (_currentSpawnTime >= _offsetSpawnTime)
            return true;

        return false;
    }

    void SpawnCloud()
    {
        cloudSpawnVFX.transform.position = _randomSpawnPoint;
        VFXManager.GetComponent<CentralVFX>().SpawnCloudVFX();
        GameObject Cloud = Instantiate(ODS7Singleton.Instance.CloudPrefab, _randomSpawnPoint, Quaternion.identity);

        Cloud.transform.parent = ODS7Singleton.Instance.EnemyEmptyParent;

        ODS7Singleton.Instance.activeClouds.Add(Cloud.GetComponentInChildren<CloudAI>());
        ODS7Actions.OnCloudSpawned();
        _spawnTimeRef = Time.time;
        _isSpawnPointSet = false;
        _spawnOffset = RandomRoundOffset();
    }

    private float RandomRoundOffset()
    {
        float offset = (float)System.Math.Round(Random.Range(0f, 0.5f), 2);
        _offsetSpawnTime = _maxSpawnTime + offset;
        return offset;
    }

    private void CallNewCloud()
    {
        _currentNextSummonTime = (Time.time - _nextSummonTimeRef);

        if (!(_currentNextSummonTime >= _maxNextSummonTime)) return;
        TargetAI.targetCloudSpawner = null;
        TargetAI = null;
        _nextSummonTimeRef = 0;
        ODS7Singleton.Instance.RequestReinforcements(this);
    }

    #endregion
    
    #region Spawn Location Methods

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

    #endregion

    #region UI Methods

    private void ActivateWrenches()
    {
        for (int i = 0; i < repairLevelImage.Length; i++)
        {
            if (!repairLevelImage[i].activeSelf)
            {
                repairLevelImage[i].SetActive(true);
                break;
            }
        }
    }

    private void ResetWrenches()
    {
        for (int i = 0; i < repairLevelImage.Length; i++)
        {
            if (repairLevelImage[i].activeSelf)
            {
                repairLevelImage[i].SetActive(false);
            }
        }
    }

    #endregion
    
    #region Disable And Restore Methods

    void DisableFactory()
    {
        myFactoryState = factoryState.Disable;
        ODS7Singleton.Instance.enabledSpawners.Remove(this);
        if (TargetAI != null)
        {
            TargetAI.CancelReturn();
            TargetAI = null;
        }
        timeMultiplier = 1f;
        VFXManager.GetComponent<CentralVFX>().CallCoroutine();
        ODS7Actions.OnFactoryDisabled();
    }

    public void RestoreFactory()
    {
        ResetWrenches();
        FixSlider.gameObject.SetActive(false);
        _currentTransformTime = 0;
        
        SpawnSlider.gameObject.SetActive(true);
        
        ODS7Singleton.Instance.spawnersDisablingList.Remove(this);
        
        if (TargetAI != null)
        {
            TargetAI.CancelReturn();
            TargetAI = null;
        }

        timeMultiplier = 1f;
        SetInteractTrue();
        _spawnOffset = RandomRoundOffset();
        myFactoryState = factoryState.Spawning;
    }

    #endregion

    #region Interaction Methods

    public override void Interact()
    {
        base.Interact();

        if (!IsInteractable) return;
        if (myFactoryState != factoryState.Spawning) return;

        SetInteractFalse();
        Unhover();
        myFactoryState = factoryState.Transforming;
        _transformTimeRef = Time.time;
        ODS7Singleton.Instance.spawnersDisablingList.Add(this);
        ODS7Singleton.Instance.RequestReinforcements(this);
    }

    #endregion

    #region Trigger Collisions Methods

    private void OnTriggerEnter(Collider other)
    {
        if (myFactoryState != factoryState.Transforming) return;
        if (!other.TryGetComponent(out CloudAI cloudAI)) return;
        if (cloudAI.isCaptured || !cloudAI.isReturningToPowerplant) return;
        ODS7Singleton.Instance.DestroyCloud(cloudAI);
        _currentRepairLevel++;
        ActivateWrenches();
        _currentTransformTime -= ODS7Singleton.Instance.transformTimeIncrease;
        _currentTransformTime = Mathf.Clamp(_currentTransformTime, 0f, _maxTransformTime);
        timeMultiplier -= 0.25f;
        timeMultiplier = Mathf.Clamp(timeMultiplier, 0.5f, 1f);
    }

    #endregion

    #region Editor Only Things (OnDrawGizmos)

    #if (UNITY_EDITOR) 
        private void OnDrawGizmos()
        {
            var position = _spawnTransform.position;
            Handles.DrawWireDisc(position, Vector3.up, maxSpawnRadius);
            Handles.DrawWireDisc(position, Vector3.up, minSpawnRadius);
        }
    #endif

    #endregion
}
