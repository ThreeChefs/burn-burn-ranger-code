
using System.Collections;
using UnityEngine;

public class BrickActiveSkill : ActiveSkill
{
    readonly float _minX = -0.35f;
    readonly float _maxX = 0.35f;
  

    protected override IEnumerator UseSkill(Transform target)
    {
        WaitForSeconds _projectileFireIntervalWait = new WaitForSeconds(Data.SpawnInterval);

        for (int i = 0; i < skillValues[SkillValueType.ProjectileCount][CurLevel - 1]; i++)
        {
            float randomDir = Random.Range(_minX, _maxX);
            SoundManager.Instance.PlaySfx(SfxName.Sfx_Projectile_Brick);
            ProjectileManager.Instance.Spawn(projectileIndex, this, new Vector2(randomDir, 1f), transform.position);

            yield return _projectileFireIntervalWait;
        }
    }
}
