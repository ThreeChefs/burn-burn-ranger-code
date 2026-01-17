
using System;
using System.Collections.Generic;

public class GrowthProgress
{
    int _normalUnlockCount = 0;       // 얼마나 열었는지
    public int NormalUnlockCount => _normalUnlockCount;


    /// <summary>
    /// UI 에서 해금하여 런타임에서 적용
    /// </summary>
    public void UnlockNormalGrowth(int unlockCount, GrowthInfo info)
    {
        _normalUnlockCount = unlockCount;
        PlayerManager.Instance.Condition[info.StatType].Add(info.Value);
    }

    public void ImportGrowthPogress(GrowthProgressSaveInfo info)
    {
        _normalUnlockCount = info.normalUnlockCount;
    }

    public GrowthProgressSaveInfo ExportGrowthProgress()
    {
        return new GrowthProgressSaveInfo
        {
            normalUnlockCount = _normalUnlockCount,
        };
    }

    /// <summary>
    /// 저장된 정보 적용
    /// </summary>
    public void ApplySavedGrowthStat(List<GrowthInfoEntry> growthInfoEntries, int unlockCount)
    {
        int count = 0;

        if (unlockCount <= 0) return;

        // 스탯 적용하기
        foreach (GrowthInfoEntry entry in growthInfoEntries)
        {
            foreach (GrowthInfo info in entry.GrowthInfos)
            {
                PlayerManager.Instance.Condition[info.StatType].Add(info.Value);
                count++;
            }

            if (count >= unlockCount)
            {
                break;
            }
        }
    }


}

[Serializable]
public class GrowthProgressSaveInfo
{
    public int normalUnlockCount;   // 일반성장 해금 정보
}
