using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolObject : PoolObject
{
    [SerializeField] private float _duration = 1.0f;
    private WaitForSeconds _wait;
    private void Start()
    {
        _wait = new WaitForSeconds(_duration);
    }

    protected override void OnEnableInternal()
    {
        StartCoroutine(DelayedDisable());
    }

    IEnumerator DelayedDisable()
    {
        yield return new WaitForSeconds(_duration);
        this.gameObject.SetActive(false);
    }

}
