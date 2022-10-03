using Base_Components;

public class AcidAbility : MeleeAttack_Base, IAbility
{
    public Insects Insect = Insects.Scorpion;

    public Poison PoisonPrefab;
    public void Use()
    {
        Attack();
    }

    public override void OnEnemyHit(Unit_Base unit)
    {
        print("On poison hit");
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
