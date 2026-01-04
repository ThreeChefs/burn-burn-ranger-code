using System;

public static class Define
{
    // 랜덤
    public static Random Random = new();

    // 스킬
    public const int SkillMaxLevel = 5;
    public const int ActiveSkillMaxCount = 6;
    public const int PassiveSkillMaxCount = 6;
    
    // 스테이지
    public const float MinMonsterSpawnDistance = 3.0f;
    public const float MaxMonsterSpawnDistance = 5.0f;
}