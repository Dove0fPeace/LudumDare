using Base_Components;
using Controls;
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

    private Player_HUD hud;
    private AI ai; 

    protected override void Start()
    {
        base.Start();
        InitiateAbility();
        ai = transform.root.GetComponent<AI>();
        
    }

    protected override void Update()
    {
        base.Update();
        hud.UpdateCooldown(ObjWithCooldown.Ability, AttackCooldown);
    }


    public bool CanUse()
    {
        return CanAttack;
    }

    public void InitiateAbility()
    {
        var ai = transform.root.GetComponent<AI>();
        if (ai != null) return;

        hud = Player_HUD.Instance;
        hud.InitUI(ObjWithCooldown.Ability);
    }


    public void Use()
    {
        Attack();
    }
}
