using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dron : MonoBehaviour
{
    Transform _target;
    [SerializeField] float _followSpeed = 3f;
    float _smoothTime = 0.12f;


    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void FixedUpdate()
    {

        Vector3 _velocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, _target.position, ref _velocity, _smoothTime);

    }


}
