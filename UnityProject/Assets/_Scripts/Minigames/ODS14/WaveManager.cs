using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    
    // [SerializeField] private float waveIntensity = 0f;
    // [SerializeField] private float waveSpeed = 0f;

    public Vector4 _steepness;
    public Vector4 _wavelength;
    public Vector4 _speed;
    public Vector4 _directions;

    private Material mat;
    private float _offset = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.LogError("WaterManager instance already exists, destroying instance");
            Destroy(this);
        }
        mat = GetComponent<MeshRenderer>().material;
        // SetWaveValues();
        SetGerstnerWaveValues();
    }

    private void Start()
    {
        // SetWaveValues();
    }

    private void Update()
    {
        // SetWaveValues();
        // _offset += waveSpeed * Time.deltaTime;
    }

    // public float GetWaveHeight(float x, float z)
    // {
    //     return waveIntensity * Mathf.Sin((x + z) + _offset);
    // }

    public Vector3 GerstnerWave(Vector3 position, float steepness, float wavelength, float speed, float direction)
    {
        direction = direction * 2 - 1;
        Vector2 d = new Vector2(Mathf.Cos(Mathf.PI * direction), Mathf.Sin(Mathf.PI * direction)).normalized;
        float k = 2 * Mathf.PI / wavelength;
        float a = steepness / k;
        float f = k * (Vector2.Dot(d, new Vector2(position.x, position.z)) - speed * Time.time);

        return new Vector3(d.x * a * Mathf.Cos(f), a * Mathf.Sin(f), d.y * a * Mathf.Cos(f));
    }
    public Vector3 GetWaveDisplacement(Vector3 position)
    {
        Vector3 offset = Vector3.zero;

        offset += GerstnerWave(position, _steepness.x, _wavelength.x, _speed.x, _directions.x);
        offset += GerstnerWave(position, _steepness.y, _wavelength.y, _speed.y, _directions.y);
        offset += GerstnerWave(position, _steepness.z, _wavelength.z, _speed.z, _directions.z);
        offset += GerstnerWave(position, _steepness.w, _wavelength.w, _speed.w, _directions.w);

        return offset;
    }

    // private void SetWaveValues()
    // {
    //     waveIntensity = mat.GetFloat("_WaveIntensity");
    //     waveSpeed = mat.GetFloat("_WaveSpeed");
    // }
    private void SetGerstnerWaveValues()
    {
        _steepness = mat.GetVector("_Steepness");
        _wavelength = mat.GetVector("_Wavelength");
        _speed = mat.GetVector("_Speed");
        _directions = mat.GetVector("_Direction");
    }
}
