using Base_Components;

public class WebAbility : RangeAttack_Base, IAbility
{
    public Insects Insect => Insects.Spider;


    public void Use()
    {
        Attack();
    }
}
