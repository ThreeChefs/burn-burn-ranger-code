using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dron : MonoBehaviour
{
    Transform _target;
    float _smoothTime = 0.3f;


    public void SetTarget(Transform target)
    {
        _target = target;
    }

    Vector3 _velocity = Vector3.zero;
    
    private void FixedUpdate()
    {

        transform.position = Vector3.SmoothDamp(transform.position, _target.position, ref _velocity, _smoothTime);

    }


}
