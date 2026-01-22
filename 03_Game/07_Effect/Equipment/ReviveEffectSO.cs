using UnityEngine;

[CreateAssetMenu(fileName = "ReviveEffectSO", menuName = "SO/Effect/Revive")]
public class ReviveEffectSO : BaseEquipmentEffectSO
{
    [field: SerializeField][field: Range(0f, 1f)] public float ReviveHpRatio { get; private set; }
    [field: SerializeField] public float InvincibleTime { get; private set; }

    public override EquipmentEffectInstance CreateInstance()
    {
        return new Instance(this);
    }

    private class Instance : EquipmentEffectInstance, IReviveEffect
    {
        private bool _used;

        public Instance(BaseEquipmentEffectSO source) : base(source) { }

        public bool CanTrigger(in BaseEffectContext context)
        {
            return !_used && context.Reason == TriggerReason.Hpzero;
        }

        public ReviveData GetReviveData()
        {
            _used = true;
            return new ReviveData
            {
                hpRatio = ((ReviveEffectSO)source).ReviveHpRatio,
                invincibleTime = ((ReviveEffectSO)source).InvincibleTime
            };
        }
    }
}

public struct ReviveData
{
    public float hpRatio;
    public float invincibleTime;
}
