using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new SkillConfig", menuName = "SO/Skill/Config")]
public class SkillConfig : ScriptableObject
{
    [SerializeField] private List<BaseSkillEffectSO> _effects = new();
    public IReadOnlyList<BaseSkillEffectSO> Effects => _effects;
}
