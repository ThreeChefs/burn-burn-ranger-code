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

    [Header("Laser Point")]
    [SerializeField] private Transform laserRoot;
    [SerializeField] private Transform[] laserPoints;
    private Transform[] lasers;

    [Header("Laser Effects")]
    [SerializeField] private GameObject pointVfxPrefab;
    [SerializeField] private Vector3 pointVfxLocalOffset = new Vector3(0f, 0f, 0f);
    private Transform[] pointVfxRoots;
    private readonly Dictionary<int, float> nextTickTime = new();
    private ParticleSystem[][] laserVfx;


    protected override bool CanRun()
    {
        if (boss == null) return false;
        if (laserPrefab == null) return false;
        if (laserRoot == null) return false;
        if (laserPoints == null || laserPoints.Length != 4) return false;
        return true;
    }

    protected override IEnumerator Execute()
    {

        CreatLaser();
        CreatePointVfx();

        SettingLaser(true);
        SetPointVfx(true);
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
        SetPointVfx(false);
        nextTickTime.Clear();
        Debug.Log("[LaserPattern] END");
    }

    private void CreatLaser()
    {
        if (lasers != null) return;

        if (laserPoints == null || laserPoints.Length != 4)
        {
            Debug.LogError("[LaserPattern] LaserPoints not set correctly");
            return;


        }

        lasers = new Transform[4];
        laserVfx ??= new ParticleSystem[4][];
        for (int i = 0; i < 4; i++)
        {
            GameObject go = Instantiate(laserPrefab, laserPoints[i]);

            go.name = $"Laser_{i}";

            var trigger = go.GetComponent<LaserTrigger>();
            if (trigger != null)
                trigger.Bind(this);
            laserVfx[i] = go.GetComponentsInChildren<ParticleSystem>(true);
            lasers[i] = go.transform;
            lasers[i].localRotation = Quaternion.identity;
            lasers[i].localScale = new Vector3(laserThickness, laserLength, 1f);
            lasers[i].localPosition = Vector3.up;

        }

        SettingLaser(false);

    }
    private void CreatePointVfx()
    {
        if (pointVfxRoots != null) return;
        if (pointVfxPrefab == null) return;
        if (lasers == null || lasers.Length != 4) return; // ✅ 레이저가 먼저 생성돼 있어야 함

        pointVfxRoots = new Transform[4];

        for (int i = 0; i < 4; i++)
        {
            if (lasers[i] == null) continue;

            GameObject vfxGo = Instantiate(pointVfxPrefab, lasers[i]); // ✅ 레이저 자식!
            vfxGo.name = $"LaserPointVfx_{i}";

            // ✅ 레이저 로컬 기준으로 위치 잡기 (빈공간 맞추는 곳)
            vfxGo.transform.localPosition = pointVfxLocalOffset;
            vfxGo.transform.localRotation = Quaternion.identity;

            pointVfxRoots[i] = vfxGo.transform;

            vfxGo.SetActive(false);
        }
    }
    private void SetPointVfx(bool on)
    {
        if (pointVfxRoots == null) return;

        for (int i = 0; i < pointVfxRoots.Length; i++)
        {
            var root = pointVfxRoots[i];
            if (root == null) continue;

            root.gameObject.SetActive(on);

            var systems = root.GetComponentsInChildren<ParticleSystem>(true);
            foreach (var ps in systems)
            {
                if (on)
                {
                    ps.Clear(true);
                    ps.Play(true);
                }
                else
                {
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    ps.Clear(true);
                }
            }
        }
    }
    private void SettingLaser(bool active)
    {
        if (lasers == null) return;

        for (int i = 0; i < lasers.Length; i++)
        {
            if (lasers[i] == null) continue;

            if (active)
            {
                // ✅ 먼저 켠 다음
                lasers[i].gameObject.SetActive(true);

                // ✅ 파티클 수동 재생 (Play On Awake OFF일 때 필수)
                if (laserVfx != null && laserVfx[i] != null)
                {
                    foreach (var ps in laserVfx[i])
                    {
                        if (ps == null) continue;
                        ps.Clear(true);
                        ps.Play(true);
                    }
                }
            }
            else
            {
                // ✅ 끌 때: 잔상 제거
                if (laserVfx != null && laserVfx[i] != null)
                {
                    foreach (var ps in laserVfx[i])
                    {
                        if (ps == null) continue;
                        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                        ps.Clear(true);
                    }
                }

                lasers[i].gameObject.SetActive(active);

            }
        }
    }

    private void UpdateLasers(float angleDeg)
    {
        if (laserRoot == null) return;
        laserRoot.localRotation = Quaternion.Euler(0, 0, angleDeg);

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
        SetPointVfx(false);
        SettingLaser(false);
        nextTickTime.Clear();
    }

}
