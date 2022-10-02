public class RangeAttack_Base : Attack_Base
{
    public Projectile_Base ProjectilePrefab;

    protected override void Start()
    {
        base.Start();
        HandsPlace = self.HandsPosition;
    }

    public override bool Attack()
    {
        if (base.Attack())
        {
            var projectile = Instantiate(ProjectilePrefab, HandsPlace.position, HandsPlace.rotation);
            projectile.Spawn(self, Damage);
            return true;
        }

        return false;
    }
}
