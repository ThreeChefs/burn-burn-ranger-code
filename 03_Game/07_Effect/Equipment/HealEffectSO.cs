using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealEffectSO", menuName = "SO/Effect/Heal")]
public class HealEffectSO : BaseEquipmentEffectSO
{
    [field: InspectorName("몬스터 조건")]
    [field: SerializeField] public KillIntervalCondition[] KillIntervalConditions;
    [field: InspectorName("지속 시간")]
    [field: SerializeField] public float Duration { get; private set; }
    [field: InspectorName("초당 회복량")]
    [field: SerializeField] public float HealPerSecond { get; private set; }

    public override EquipmentEffectInstance CreateInstance()
    {
        return new Instance(this);
    }

    private class Instance : EquipmentEffectInstance, IThresholdEffect
    {
        private readonly KillIntervalCondition[] _conditions;
        private readonly float _duration;
        private readonly float _healPerSecond;

        public Instance(HealEffectSO source) : base(source)
        {
            _conditions = source.KillIntervalConditions;
            _duration = source.Duration;
            _healPerSecond = source.HealPerSecond;
        }

        public bool TryConsume(in KillEffectContext context, out List<BaseBuff> buffs)
        {
            buffs = null;

            for (int i = 0; i < _conditions.Length; i++)
            {
                int triggerCount = context.KillStatus.ConsumeInterval(_conditions[i].Type, _conditions[i].Interval);

                if (triggerCount == 0) continue;

                buffs ??= new();
                for (int n = 0; n < triggerCount; n++)
                {
                    buffs.Add(new HealBuff(_duration, _healPerSecond));
                }
            }

            return buffs.Count > 0;
        }
    }
}
