using System;

public static class Define
{
    // 랜덤
    public static Random Random = new();

    public static float RandomRange(float min, float max)
    {
        return min + (float)Random.NextDouble() * (max - min);
    }

    // 스킬
    public const int SkillMaxLevel = 5;
    public const int ActiveSkillMaxCount = 6;
    public const int PassiveSkillMaxCount = 6;
    public const int SelectableSkillMaxCount = 3;
    
    // 스테이지
    // todo : define이 아니라 맵나오면 맵 범위에 맞게 맵에서 뽑아 써야할 듯 함.
    public const float MinMonsterSpawnDistance = 20.0f;
    public const float MaxMonsterSpawnDistance = 30.0f;

    public const int MapSize = 40;
}