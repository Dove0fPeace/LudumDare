using Base_Components;
using Controls;
using System.Collections;
using UnityEngine;

public class Bombardir_Ability : MeleeAttack_Base, IAbility
{
    [Header("Ability Settings")]
    public float AbilityTickTime;
    public int AbilityTickCount;

    private AI ai; 

    protected override void Start()
    {
        base.Start();
        ai = transform.root.GetComponent<AI>();
        InitiateAbility();
        
    }

    public bool CanUse()
    {
        return CanAttack;
    }

    public void InitiateAbility()
    {
        if (ai != null) return;

        Player_HUD.Instance.InitUI(ObjWithCooldown.Ability, true, Timer_AttackCooldown);
    }

    public void Use()
    {
        StartCoroutine(BombardirAbilityAttack());
    }

    private IEnumerator BombardirAbilityAttack()
    {
        for (int i = 0; i < AbilityTickCount; i++)
        {
            yield return new WaitForSeconds(AbilityTickTime);
            Attack();
        }
    }
}
