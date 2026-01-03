using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬을 저장하고 관리하는 컨트롤러
/// </summary>
public class SkillController : MonoBehaviour
{
    [SerializeField] private SoDatabase _skillDatabase;
    private List<SkillData> _cache;

    // 스킬 상태 관리
    protected readonly List<BaseSkill> havingSkills = new();
    private BaseSkill _curSkill = null;

    private void Awake()
    {
        _cache = _skillDatabase.GetDatabase<SkillData>();
    }

    /// <summary>
    /// [public] 스킬 획득
    /// </summary>
    /// <param name="id"></param>
    /// <param name="baseSkill"></param>
    public bool TrySelectSkill<T>(int id) where T : BaseSkill
    {
        if (_cache.Count < id)
        {
            Logger.LogWarning($"얻을 수 없는 스킬 데이터: {id}");
            return false;
        }

        BaseSkill find = havingSkills.Find(skill => skill == _cache[id]);

        if (havingSkills)
        {
            Logger.LogWarning("이미 존재하는 스킬");
            return false;
        }

        T baseSkill = gameObject.AddComponent<T>();

        havingSkills.Add(baseSkill);

        baseSkill.Init(_cache[id]);
        baseSkill.OnStartSkill += HandleStartSkill;
        baseSkill.OnEndSkill += HandleEndSkill;

        return true;
    }

    #region curSkill 관리
    /// <summary>
    /// 스킬 사용 시 curSkill에 값 저장
    /// </summary>
    /// <param name="baseSkill"></param>
    private bool HandleStartSkill(BaseSkill baseSkill)
    {
        if (_curSkill != null)
        {
            Logger.LogWarning("이미 다른 스킬 사용 중");
            return false;
        }
        _curSkill = baseSkill;
        return true;
    }

    /// <summary>
    /// 스킬 사용 완료 시 curSkill에서 값 제거
    /// </summary>
    private void HandleEndSkill()
    {
        _curSkill = null;
    }
    #endregion

    #region 에디터 전용
#if UNITY_EDITOR
    protected virtual void Reset()
    {
        if (_skillDatabase == null)
        {
            _skillDatabase = AssetLoader.FindAndLoadByName<SkillDatabase>("SkillDatabase");
        }
    }
#endif
    #endregion
}
