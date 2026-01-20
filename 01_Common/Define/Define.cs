using System;
using UnityEngine;

public static class Define
{
    // 랜덤
    public static System.Random Random = new();

    public static float RandomRange(float min, float max)
    {
        return min + (float)Random.NextDouble() * (max - min);
    }

    // 스킬
    // 시스템
    public const int InfinitePass = -100;

    public const int SkillMaxLevel = 5;
    public const int ActiveSkillMaxCount = 6;
    public const int PassiveSkillMaxCount = 6;
    public const int SelectableSkillMaxCount = 3;

    // 플레이어 맥스 레벨
    public const int PlayerMaxLevel = 100;
    public const int StageMaxLevel = 50;

    // 패시브
    public const float HealTime = 1f;

    // 스테이지
    // todo : define이 아니라 맵나오면 맵 범위에 맞게 맵에서 뽑아 써야할 듯 함.
    public const float MinMonsterSpawnDistance = 10.0f;
    public const float MaxMonsterSpawnDistance = 15.0f;

    // 몬스터 최대 소환
    public const int MaxMonsterSpawnCount = 300;

    // 맵
    public const int MapSize = 40;
    public const int TilemapCount = 4;

    // 태그
    public const string PlayerTag = "Player";

    // 레이어 이름
    public static int MonsterLayer = LayerMask.NameToLayer("Monster");
    public static int WallLayer = LayerMask.NameToLayer("Wall");
    public static int PlayerLayer = LayerMask.NameToLayer("Player");

    // 디자인 - 홈
    public static float OriginPosY = 0f;
    public static float TargetPosY = 50f;
    public static Color OriginColor = new(0.20784315f, 0.20784315f, 0.3019608f);
    public static Color TargetColor = new(1f, 0.64705884f, 0.19607845f);
    public static float OriginScale = 1f;
    public static float TargetScale = 1.4f;
    public static float Duration = 1f;

    // 모바일 확인
    /// <summary>
    /// 에디터에서는 항상 true,
    /// 빌드에서는 모바일(Android/iOS)만 true
    /// </summary>
    public static bool IsMobileRuntime
    {
        get
        {
#if UNITY_EDITOR
            return true;
#else
            return Application.platform == RuntimePlatform.Android
                || Application.platform == RuntimePlatform.IPhonePlayer;
#endif
        }
    }

    /// <summary>
    /// 모바일 전용 UI/입력 사용 가능 여부
    /// (에디터에서는 강제 허용)
    /// </summary>
    public static bool EnableMobileUI => IsMobileRuntime;
}