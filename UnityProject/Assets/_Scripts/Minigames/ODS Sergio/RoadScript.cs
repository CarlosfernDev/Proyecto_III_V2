using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadScript : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector3 carDir;

    [SerializeField] private float timeBeetweenSpawns;
    private float timer;
    [SerializeField] private float carSpeed;
    [SerializeField] private float distanceToDestroy;

    private GameObject go = null;

    [SerializeField] private List<GameObject> goList;
    private List<int> index;

    private TroncoParent script;


    void Start()
    {
        goList = new List<GameObject>();
        index = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCars();
        if (timeBeetweenSpawns<timer)
        {
            
            go = Instantiate(car,spawnPoint.position,Quaternion.identity);
            
            script  = go.GetComponentInChildren<TroncoParent>(true);
            if (script != null)
            {
               
                script.speed = carSpeed;
                script.dir = carDir;

            }
            goList.Add(go);
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
        
    }


    void MoveCars()
    {
        if(goList.Count == 0) { return; }


        for (int i = 0; i < goList.Count; i++)
        {
            if (Vector3.Distance(goList[i].transform.position, spawnPoint.transform.position) > distanceToDestroy)
            {
                Destroy(goList[i].transform.gameObject);
                goList.RemoveAt(i);
            }
            else
            {
                goList[i].transform.position += carDir * carSpeed * Time.deltaTime;
            }
        }
        /*
            if (Vector3.Distance(go.transform.position,spawnPoint.transform.position)> distanceToDestroy)
            {
                Destroy(go.gameObject);
                go = null;
            }
            else
            {
                go.transform.position += carDir * carSpeed * Time.deltaTime;
            }
        */
            
    }

       
}

