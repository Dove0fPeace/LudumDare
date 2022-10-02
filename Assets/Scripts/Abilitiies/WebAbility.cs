using Base_Components;

public class WebAbility : RangeAttack_Base, IAbility
{
    public Insects InsectType => Insects.Spider;

    protected override void Start()
    {
        base.Start();
    }

    public void Use()
    {
        Attack();
    }
}