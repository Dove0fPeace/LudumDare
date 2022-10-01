using System.Collections;
using System.Collections.Generic;
using Base_Components;
using UnityEngine;

public class Ability_Base : MonoBehaviour, IAbility
{
    public BodyPart Part = BodyPart.Abilitiy;

    public float AbilityCooldown;
    private float abilityTime = 0;
    private bool CanAbility => abilityTime <= 0;

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
}
