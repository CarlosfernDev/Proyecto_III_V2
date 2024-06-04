using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAnimation : MonoBehaviour
{
    [SerializeField] public Transform Follow;
    [SerializeField] public float speedFollow;
    [SerializeField] public float distanceBack;
    public AnimationCurve myCurve;
    private Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        if (Follow != null) return;

        if (UnlockablesManager.instance == null)
            Debug.LogError("Te falta designar a que tiene que seguir la Pet de forma manual o poner un UnlockableManager");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetBehindPos(distanceBack);
        if (Vector3.Distance(pos,transform.position)<1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, speedFollow * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, speedFollow*2 * Time.deltaTime);
        }


        transform.position = new Vector3(transform.position.x, Follow.transform.position.y+ (myCurve.Evaluate((Time.time % myCurve.length))/2), transform.position.z);
        transform.LookAt(Follow);
    }

    void GetBehindPos(float distance)
    {

        pos = Follow.position - (Follow.forward * distance);
    }
}
