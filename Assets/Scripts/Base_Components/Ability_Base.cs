using Base_Components;
using UnityEngine;

public class Ability_Base : MonoBehaviour, IAbility
{
    public BodyPart Part = BodyPart.Abilitiy;

    private Player_HUD hud;

    public Sprite AbilityUISprite;

    public float AbilityCooldown;
    private float abilityTime = 0;
    private bool CanAbility => abilityTime <= 0;

    private void Start()
    {
        InitiateAbility();
    }
    private void Update()
    {
        if (abilityTime > 0)
            abilityTime -= Time.deltaTime;
    }

    public virtual void Use()
    {
        if(!CanAbility) return;
        print("Use ability");
        abilityTime = AbilityCooldown;
    }

    public bool CanUse()
    {
        return false;
    }

    public void InitiateAbility()
    {
        hud.InitUI(ObjWithCooldown.Ability, AbilityUISprite);
    }
}
