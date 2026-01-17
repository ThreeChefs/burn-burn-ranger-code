
public class GrowthProgress
{
    int _normalUnlockCount = 5;       // 얼마나 열었는지
    public int NormalUnlockCount => _normalUnlockCount;

    
    public void ImportStageProgress(GrowthProgressSaveInfo info)
    {
        _normalUnlockCount = info.normalUnlockCount;
    }
 
}

public class GrowthProgressSaveInfo
{
    public int normalUnlockCount;   // 일반성장 해금 정보
}
