using UnityEngine;

/// <summary>
/// Hp를 기준으로 Stat 버프 주기
/// </summary>
[CreateAssetMenu(fileName = "new HpConditionStatEffectSO", menuName = "SO/Effect/Hp Condition Stat Buff")]
public class HpConditionStatEffectSO : BaseEquipmentEffectSO
{
    [field: SerializeField] public HpRatioCondition HpRatioCondition { get; private set; }
    [field: SerializeField] public StatModifier StatModifier { get; private set; }

    public override EquipmentEffectInstance CreateInstance()
    {
        return new Instance(this);
    }

    private class Instance : EquipmentEffectInstance
    {
        private readonly HpRatioCondition _condition;
        private readonly StatModifier _modifier;
        private bool _registered;

        public Instance(HpConditionStatEffectSO source) : base(source)
        {
            _condition = source.HpRatioCondition;
            _modifier = source.StatModifier;
        }

        public override void OnStageStart(BuffSystem system)
        {
            if (_registered) return;
            _registered = true;

            system.Add(Key, new HpConditionStatBuff(_condition, _modifier));
        }
    }
}
