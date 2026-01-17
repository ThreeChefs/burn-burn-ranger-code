using UnityEngine;

public class BossVfxController : MonoBehaviour
{
    [SerializeField] private ParticleSystem auraVfx; // 보스에 붙은 파티클

    public void SetAura(bool on)
    {
        if (auraVfx == null) return;

        if (on)
        {
            auraVfx.gameObject.SetActive(true);   // 꺼져있을 수 있으니
            auraVfx.Play(true);
        }
        else
        {
            auraVfx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            // 필요하면 완전히 숨김
            // auraVfx.gameObject.SetActive(false);
        }
    }
}
