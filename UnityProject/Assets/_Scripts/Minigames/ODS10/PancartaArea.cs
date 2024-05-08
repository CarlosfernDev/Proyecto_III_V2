using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancartaArea : MonoBehaviour
{
    public int Score;
    public Jurado _jurado;

    // Start is called before the first frame update
    void Start()
    {
        Score = 2;
        _jurado.ChangeImage(Score);
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void OnTriggerEnter(Collider other)
    {
        if (!ODS10Singleton.Instance.CartelDictionary.ContainsKey(other.gameObject)) return;

        ScriptableObjectComponente Script = ODS10Singleton.Instance.CartelDictionary[other.gameObject];

        Score += (Script.IsGood) ? 1 : -1;
        Score = Mathf.Clamp(Score, 0, 4);
       Debug.Log("INSIDE" + other.gameObject.name);
        _jurado.ShowScore(Score);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!ODS10Singleton.Instance.CartelDictionary.ContainsKey(other.gameObject)) return;

        ScriptableObjectComponente Script = ODS10Singleton.Instance.CartelDictionary[other.gameObject];

        Score -= (Script.IsGood) ? 1 : -1;
        Score = Mathf.Clamp(Score, 0, 4);

        Debug.Log("OUTSIDE" + other.gameObject.name);
        _jurado.ShowScore(Score);
    }

}
