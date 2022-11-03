using Base_Components;
using Controls;
using UnityEngine;

public class Ability_Base : MonoBehaviour, IAbility
{
    public BodyPart Part = BodyPart.Abilitiy;

    private Player_HUD hud;

    public Sprite AbilityUISprite;

    private void Start()
    {
        hud = Player_HUD.Instance;
        InitiateAbility();
    }


    public virtual void Use()
    {
        hud.TryUseOnCooldown(ObjWithCooldown.Ability);
    }

    public bool CanUse()
    {
        return true;
    }
    
    public void InitiateAbility()
    {
        var ai = transform.root.GetComponent<AI>();
        if (ai != null) return;
        hud.InitUI(ObjWithCooldown.Ability, false, null);
    }
    
}
