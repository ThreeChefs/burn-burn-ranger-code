using UnityEngine;

public class WhistleArrowProjectile : PlayerProjectile
{
    [SerializeField] private float _maxTurnDegPerSec = 30f; // 회전 한계

    public override void Init(ActiveSkill activeSkill, PoolObjectData originData)
    {
        base.Init(activeSkill, originData);
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override void SetGuidance()
    {
        if (Target == null || !Target.gameObject.activeSelf)
        {
            Target = MonsterManager.Instance.GetRandomMonster();
            if (Target == null)
            {
                MoveDir = Vector2.zero;
                return;
            }
        }

        Vector2 toTarget = Target.position - transform.position;

        float currentAngle = transform.eulerAngles.z;
        float targetAngle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;

        float angularSpeedDeg = _maxTurnDegPerSec * Mathf.Rad2Deg;

        float newAngle = Mathf.MoveTowardsAngle(
            currentAngle,
            targetAngle,
            angularSpeedDeg * Time.fixedDeltaTime
        );

        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
        MoveDir = transform.right;
    }
}
