using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPattern : BossPatternBase
{
    [Header("Laser")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserLength = 30f;
    [SerializeField] private float laserThickness = 2f;
    private Transform laserPoint;

    [Header("Laser Setting")]
    [SerializeField] private float UsingTime = 3f;
    [SerializeField] private float rotationTime = 5f;
    [SerializeField] private float rotationSpeed = 90f;

    [Header("Attack CalCulator")]
    [SerializeField] private float damageMultiplierPerTick = 0.5f;
    [SerializeField] private float tickInterval = 0.2f;

    private Transform[] lasers;
    private readonly Dictionary<int, float> nextTickTime = new();


    protected override bool CanRun()
    {
        return boss != null && laserPrefab != null;
    }

    protected override IEnumerator Execute()
    {
        CreatLaser();
        SettingLaser(true);
        Debug.Log("[LaserPattern] START");
        float t = 0f;
        while (t < UsingTime)
        {
            t += Time.deltaTime;
            UpdateLasers(0f);
            yield return null;
        }

        t = 0f;
        float angle = 0f;
        while (t < rotationTime)
        {
            t += Time.deltaTime;
            angle -= rotationSpeed * Time.deltaTime;
            UpdateLasers(angle);
            yield return null;
        }
        SettingLaser(false);
        nextTickTime.Clear();
        Debug.Log("[LaserPattern] END");
    }

    private void CreatLaser()
    {
        if (lasers != null)
        {
            return;
        }
        GameObject LaserPoint = new GameObject("LaserPoint");
        LaserPoint.transform.SetParent(transform, false);
        laserPoint = LaserPoint.transform;
        lasers = new Transform[4];

        for (int i = 0; i < 4; i++)
        {
            GameObject gameObject = Instantiate(laserPrefab, laserPoint);
            gameObject.name = $"Laser_{i}";

            var trigger = gameObject.GetComponent<LaserTrigger>();
            if (trigger != null)
            {
                trigger.Bind(this);
            }
            lasers[i] = gameObject.transform;
        }
        SetupLaser();
        SettingLaser(false);

    }

    private void SettingLaser(bool active)
    {
        if (lasers == null) return;
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].gameObject.SetActive(active);
        }
    }

    private void UpdateLasers(float angleDeg)
    {
        laserPoint.localRotation = Quaternion.Euler(0, 0, angleDeg);

    }

    internal void ApplyTickDamage(Collider2D other)
    {
        if (!other.TryGetComponent<IDamageable>(out var dmg))
            return;

        int id = other.GetInstanceID();
        float now = Time.time;

        if (nextTickTime.TryGetValue(id, out float next) && now < next)
            return;

        nextTickTime[id] = now + tickInterval;

        float damage = boss.Attack * damageMultiplierPerTick;
        dmg.TakeDamage(damage);
    }
    private void OnDisable()
    {
        if (lasers == null)
        {
            return;
        }

        SettingLaser(false);
        nextTickTime.Clear();
    }

    private void SetupLaser()
    {
        Debug.Log($"[LaserPattern] length={laserLength}, scale={lasers[0].localScale}");
        float half = laserLength * 0.5f;

        // 동
        lasers[0].localPosition = Vector3.right * half;
        lasers[0].localRotation = Quaternion.Euler(0, 0, 0);

        // 서
        lasers[1].localPosition = Vector3.left * half;
        lasers[1].localRotation = Quaternion.Euler(0, 0, 180);

        // 남
        lasers[2].localPosition = Vector3.down * half;
        lasers[2].localRotation = Quaternion.Euler(0, 0, -90);

        // 북
        lasers[3].localPosition = Vector3.up * half;
        lasers[3].localRotation = Quaternion.Euler(0, 0, 90);

        // 크기(길이/두께)
        for (int i = 0; i < 4; i++)
            lasers[i].localScale = new Vector3(laserLength, laserThickness, 1f);
        Debug.Log($"[LaserPattern] AFTER length={laserLength}, scale={lasers[0].localScale}");
    }
}
