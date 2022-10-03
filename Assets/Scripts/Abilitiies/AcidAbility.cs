using Base_Components;

public class AcidAbility : RangeAttack_Base, IAbility
{
    public Insects Insect = Insects.Scorpion;
    public void Use()
    {
        Attack();
    }

}
