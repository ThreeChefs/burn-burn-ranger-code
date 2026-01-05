using System;
using UnityEngine;

public class LevelSystem
{
    // 레벨
    public int Level { get; private set; }
    public float CurrentExp { get; private set; }
    public float RequiredExp { get; private set; }

    // 이벤트
    public event Action<int> OnLevelChanged;
    public event Action<float> OnExpChanged;

    public LevelSystem(int level, float currentExp)
    {
        Level = level;
        CurrentExp = currentExp;
        RequiredExp = GetRequiredExp(Level);
    }

    public void OnDestroy()
    {
        // 이벤트 초기화
        OnLevelChanged = null;
        OnExpChanged = null;
    }

    /// <summary>
    /// [public] 경험치 회득
    /// </summary>
    /// <param name="exp"></param>
    public void AddExp(float exp)
    {
        CurrentExp += exp;
        while (CurrentExp > RequiredExp)
        {
            LevelUp();
        }
        OnExpChanged?.Invoke(CurrentExp / RequiredExp);
    }

    /// <summary>
    /// 레벨업 경험치 받아오기
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    private float GetRequiredExp(int level)
    {
        float baseExp = 100f;
        float quad = 20f * level * level;
        float expo = Mathf.Pow(1.05f, level);

        return (baseExp + quad) * expo;
    }

    private void LevelUp()
    {
        CurrentExp -= RequiredExp;
        Level++;
        OnLevelChanged?.Invoke(Level);
        RequiredExp = GetRequiredExp(Level);
    }
}
