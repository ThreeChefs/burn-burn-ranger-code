/// <summary>
/// 풀링해서 쓸 오브젝트 타입들 추가
/// Pool 을 Prefab으로 만들어서 PoolManager 에 넣어놓고 쓰기
/// Pool Prefab 이름을 PoolType의 이름과 동일하게 맞추기 
/// </summary>
public enum PoolType
{
    // 예시
    TestObjectPool,
    
    // 오브젝트
    HowlWindPool,
    
    
    // 파티클
    ParticleCryPool,
    ParticleCryBindingPool,
    
}
