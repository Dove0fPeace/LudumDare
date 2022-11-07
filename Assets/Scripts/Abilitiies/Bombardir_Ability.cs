using Base_Components;
using System.Collections;
using UnityEngine;

public class Bombardir_Ability : MeleeAttack, IAbility
{
    [Header("Ability Settings")]
    public float AbilityTickTime;
    public int AbilityTickCount;

    public bool CanUse()
    {
        return CanAttack;
    }
    
    public void Use()
    {
        if(base.Attack())
            StartCoroutine(BombardirAbilityAttack());
    }

    private IEnumerator BombardirAbilityAttack()
    {
        for (int i = 0; i < AbilityTickCount - 1; i++)
        {
            yield return new WaitForSeconds(AbilityTickTime);
            StartCoroutine(CheckRaycastOnAttack());
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        StopAllCoroutines();
    }
}
