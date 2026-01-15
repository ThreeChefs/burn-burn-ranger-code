public class StageScene : BaseScene
{
    private void Start()
    {

        // todo : 임시! 플레이어, 스테이지 정보 읽고 사용될 수 있는 것들을 쭉 등록해줘야함.
        ProjectileManager.Instance.UsePool(ProjectileDataIndex.KunaiProjectileData);
        ProjectileManager.Instance.UsePool(ProjectileDataIndex.RangedAttack);
        ProjectileManager.Instance.UsePool(ProjectileDataIndex.FireBombProjectileData);
        ProjectileManager.Instance.UsePool(ProjectileDataIndex.RocketProjectileData);
        ProjectileManager.Instance.UsePool(ProjectileDataIndex.GhostShurikenProjectileData);
        ProjectileManager.Instance.UsePool(ProjectileDataIndex.SoccerBallProjectileData);
        ProjectileManager.Instance.UsePool(ProjectileDataIndex.QuantumBallProjectileData);
        ProjectileManager.Instance.UsePool(ProjectileDataIndex.DurianProjectileData);
        ProjectileManager.Instance.UsePool(ProjectileDataIndex.DrillShotProjectileData);

        CommonPoolManager.Instance.UsePool(CommonPoolIndex.DamageText);
    }
}
