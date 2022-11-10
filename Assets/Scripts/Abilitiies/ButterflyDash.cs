using Base_Components;

public class ButterflyDash : Dash_Base, IAbility
{
    public bool CanUse()
    {
        return CanDash;
    }

    protected override void Start()
    {
        base.Start();
        InitiateAbility();
    }

    private void InitiateAbility()
    {
        if(hud != null)
            hud.InitUI(ObjWithCooldown.Ability, true, dashTimer);
    }
    public void Use()
    {
        if(!CanDash) return;
        Dash();
    }
}
