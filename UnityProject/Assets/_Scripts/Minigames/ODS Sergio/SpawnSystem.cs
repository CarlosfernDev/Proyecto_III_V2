using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] GameObject TriggerGO;
    // Start is called before the first frame update
    void Awake()
    {
        spawnPoint = transform.Find("SpawnPoint").transform.position;
    }

    public void TriggerOnChildEnterer(GameObject child)
    {
        GameManagerSergio.Instance.actualSpawnPoint = spawnPoint;
    }
}
