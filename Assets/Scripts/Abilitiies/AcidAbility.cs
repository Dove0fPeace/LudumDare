using Base_Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidAbility : MeleeAttack_Base, IAbility
{
    public Insects InsectType = Insects.Scorpion;

    public Poison PoisonPrefab;
    public void Use()
    {
        Attack();
    }

    public override void OnEnemyHit(Unit_Base unit)
    {
        print("On poison hit");
        base.OnEnemyHit(unit);
        Poison existPoison = unit.transform.GetComponent<Poison>();
        if(existPoison != null)
        {
            Destroy(existPoison);
        }

        Poison newPoison = Instantiate(PoisonPrefab, unit.transform);
        newPoison.Target = unit;            
    }
}
