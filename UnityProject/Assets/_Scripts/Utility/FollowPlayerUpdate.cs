using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerUpdate : MonoBehaviour
{
    public Transform _transform;
    public Transform _playerTransform;

    [SerializeField] private bool FollowPosition = true;
    [SerializeField] private bool FollowRotation = false;
    [SerializeField] private bool FollowScale = false;

    public bool AxisX = true;
    public bool AxisY = true;
    public bool AxisZ = true;

    private void Update()
    {
        if (FollowPosition) _transform.position = new Vector3(_playerTransform.position.x * (AxisX ? 1 : 0), _playerTransform.position.y * (AxisY ? 1 : 0), _playerTransform.position.z * (AxisZ ? 1 : 0));

        if (FollowRotation) _transform.rotation = _playerTransform.rotation;

        if (FollowScale) _transform.localScale = _playerTransform.localScale;
    }
}
