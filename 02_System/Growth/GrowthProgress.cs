
using System;

public class GrowthProgress
{
    int _normalUnlockCount = 0;       // 얼마나 열었는지
    public int NormalUnlockCount => _normalUnlockCount;

    
    public void UnlockNormalGrowth(int unlockCount)
    {
        _normalUnlockCount = unlockCount;
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
 
}

[Serializable]
public class GrowthProgressSaveInfo
{
    public int normalUnlockCount;   // 일반성장 해금 정보
}
