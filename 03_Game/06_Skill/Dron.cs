using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dron : MonoBehaviour
{
    Transform _target;
    [SerializeField] float _followSpeed = 3f;
    float _smoothTime = 0.08f;


    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void LateUpdate()
    {

        Vector3 _velocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position,_target.position,ref _velocity, _smoothTime);

        //this.transform.position =
        //    Vector3.MoveTowards(transform.position,_target.position, Time.deltaTime);



    }


}
