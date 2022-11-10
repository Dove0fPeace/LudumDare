namespace Projectiles
{
    public class AcidBall: Projectile_Base
    {
        public Poison PoisonPrefab;
        protected override void OnEnemyHit(Unit_Base unit)
        {
            base.OnEnemyHit(unit);
            Poison existPoison = unit.transform.GetComponent<Poison>();
            if(existPoison != null)
            {
                Destroy(existPoison);
            }

            Poison newPoison = Instantiate(PoisonPrefab, unit.transform);
            newPoison.Target = unit;            
        }
    }
}