using Base_Components;

public class RangeAttackAbility : RangeAttack, IAbility
{
    public Insects Insect;

    public bool CanUse()
    {
        return CanAttack;
    }

    public void Use()
    {
        Attack();
    }
}
