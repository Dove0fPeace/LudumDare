using Base_Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Bombardir_Ability : MeleeAttack_Base, IAbility
{
    [Header("Ability Settings")]
    public Sprite AbilityUISprite;
    public float AbilityTickTime;
    private float CurrentTickTIme;
    public int AbilityTickCount;
    private int currentTickCount;

    private bool isUse = false;

    private float currentCooldown;

    private Player_HUD hud;

    protected override void Start()
    {
        base.Start();
        InitiateAbility();
    }

    protected override void Update()
    {
        base.Update();
        if(isUse)
        {
            CurrentTickTIme -= Time.deltaTime;
            if(CurrentTickTIme <= 0)
            {
                currentTickCount--;
                if(currentTickCount <= 0)
                {
                    isUse = false;
                    
                }
                CheckRaycastOnAttack();
                print("Attack");
                CurrentTickTIme = AbilityTickTime;
            }
        }
        currentCooldown = attackTime / AttackCooldown;
        if(currentCooldown <= 0)
        {
            currentCooldown = 0;
        }
        hud.UpdateCooldown(ObjWithCooldown.Ability, currentCooldown);       
    }


    public bool CanUse()
    {
        return CanAttack;
    }

    public void InitiateAbility()
    {
        hud = Player_HUD.Instance;
        hud.InitUI(ObjWithCooldown.Ability, AbilityUISprite);
    }


    public void Use()
    {
        Attack();
    }

    public override bool Attack()
    {

        currentTickCount = AbilityTickCount - 1;
        isUse = true;
        return base.Attack();
    }
}
