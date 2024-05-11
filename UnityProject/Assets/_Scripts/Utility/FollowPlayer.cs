using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform _transform;
    public Transform _playerTransform;

    [SerializeField] private bool FollowPosition = true;
    [SerializeField] private bool FollowRotation = false;
    [SerializeField] private bool FollowScale = false;

    private void FixedUpdate()
    {
        if(FollowPosition)  _transform.position = _playerTransform.position;

        if (FollowRotation) _transform.rotation = _playerTransform.rotation;

        if (FollowScale) _transform.localScale = _playerTransform.localScale;
    }
}
