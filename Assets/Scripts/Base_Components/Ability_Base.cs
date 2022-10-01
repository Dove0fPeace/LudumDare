using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Base : MonoBehaviour
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

    public virtual void UseAbility()
    {
        if(!CanAbility) return;
        print("Use ability");
        abilityTime = AbilityCooldown;
    }
}
