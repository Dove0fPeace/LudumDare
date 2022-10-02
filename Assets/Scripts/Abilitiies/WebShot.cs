using Base_Components;

public class WebShot : RangeAttack_Base, IAbility
{
    public Insects InsectType => Insects.Spider;

    public void Use()
    {
        Attack();
    }
}
