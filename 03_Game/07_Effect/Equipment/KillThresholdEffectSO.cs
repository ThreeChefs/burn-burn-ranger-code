using UnityEngine;

/// <summary>
/// 몬스터 n 마리 처치 시 stat 상승 효과
/// </summary>
[CreateAssetMenu(fileName = "new KillThresholdEffectSO", menuName = "SO/Effect/Kill Threshold")]
public class KillThresholdEffectSO : BaseEquipmentEffectSO
{
    [field: InspectorName("몬스터 조건")]
    [field: SerializeField] public KillIntervalCondition[] KillIntervalConditions { get; private set; }
    [field: SerializeField] public StatModifier StatModifier { get; private set; }
    [field: InspectorName("지속 시간")]
    [field: SerializeField] public float Duration { get; private set; }

    public override EquipmentEffectInstance CreateInstance()
    {
        return new Instance(this);
    }

    private class Instance : EquipmentEffectInstance, IThresholdEffect
    {
        private readonly KillIntervalCondition[] _conditions;
        private readonly StatModifier _modifier;
        private readonly float _duration;

        public Instance(KillThresholdEffectSO source) : base(source)
        {
            _conditions = source.KillIntervalConditions;
            _duration = source.Duration;
            _modifier = source.StatModifier;
        }

        public bool TryConsume(in KillEffectContext context)
        {
            bool register = false;

            for (int i = 0; i < _conditions.Length; i++)
            {
                int triggerCount = context.KillStatus.ConsumeInterval(_conditions[i].Type, _conditions[i].Interval);

                if (triggerCount == 0) continue;

                for (int n = 0; n < triggerCount; n++)
                {
                    context.Base.BuffSystem.Add(Key, new StatBuff(_duration, _modifier));
                    register = true;
                }
            }

            return register;
        }
    }
}
