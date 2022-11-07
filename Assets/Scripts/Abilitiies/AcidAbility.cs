using Base_Components;

public class AcidAbility : RangeAttack, IAbility
{
    public bool CanUse()
    {
        return CanAttack;
    }

    public void Use()
    {
        Attack();
    }
}
