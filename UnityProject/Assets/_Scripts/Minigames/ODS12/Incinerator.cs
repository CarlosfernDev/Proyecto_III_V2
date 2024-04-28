using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incinerator : MonoBehaviour
{
    [SerializeField] private float timeToIncinerate = 2f;
    private float _timer = 0f;
    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out GarbageScript thisGarbage)) return;
        _timer += Time.deltaTime;
        if (_timer >= timeToIncinerate)
        {
            ODS12Singleton.Instance.RemoveScore(ODS12Singleton.Instance.scoreRemove);
            ODS12Singleton.Instance.gameTimer.RestTime(ODS12Singleton.Instance.timePenalty);
            ODS12Singleton.Instance.OnGarbageDelivered.Invoke();
            Destroy(thisGarbage.gameObject);
            _timer = 0;
        }
    }
}