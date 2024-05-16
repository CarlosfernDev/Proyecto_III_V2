using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    public static Vector3 TeleportToThisPosition;
    public GameManager.GameState CustomState;

    public LookAtZAxis ArrowAxis;
    public List<GameObject> GPSPositions;

    private void Awake()
    {
        GameManager.Instance.isPlaying = true;

        if (CustomState != 0)
        {
            GameManager.Instance.NextState((int)CustomState);
        }
        else
        {
            GameManager.Instance.UpdateState();
        }

        // GameManager.Instance.state = GameManager.GameState.PostGame;

        //GameManager.Instance.NextState((int)CustomState);

        if (GPSPositions.Count < (int)GameManager.Instance.state) ArrowAxis.gameObject.SetActive(false);
        else if (GPSPositions[(int)GameManager.Instance.state] == null) ArrowAxis.gameObject.SetActive(false);
        else ArrowAxis.TargetTransform = GPSPositions[(int)GameManager.Instance.state].transform;

        GameManager.Instance.UpdateStars();
    }

    private void Start()
    {
        Debug.Log(GameManager.Instance.state);

        Debug.Log("Se intento tpear " + TeleportToThisPosition);
        if(!Vector3.Equals( TeleportToThisPosition, Vector3.zero))
        {
            TestInputs player = GameManager.Instance.playerScript;
            player.rb.position = TeleportToThisPosition;
            TeleportToThisPosition = Vector3.zero;
        }
    }
}
