using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class BaseEquipmentEffectSO : ScriptableObject
{
    [SerializeField, HideInInspector] private BuffKey _buffKey; // SO 생성 시 GUID 값 저장
    [SerializeField, HideInInspector] private bool _keyInitialized;
    public BuffKey BuffKey => _buffKey;

    [field: SerializeField] public BuffEndCondition BuffEndType { get; private set; }
    public abstract EquipmentEffectInstance CreateInstance();

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        if (!_keyInitialized)
        {
            _buffKey = BuffKeyGenerator.Generate(this);
            _keyInitialized = true;
            EditorUtility.SetDirty(this);
        }
    }
#endif
}

public abstract class EquipmentEffectInstance
{
    public BuffKey Key { get; }
    protected BuffEndCondition endCondition;
    protected readonly BaseEquipmentEffectSO source;

    protected EquipmentEffectInstance(BaseEquipmentEffectSO source)
    {
        Key = source.BuffKey;
        endCondition = source.BuffEndType;
        this.source = source;
    }

    public virtual void OnStageStart(BuffSystem system) { }
}