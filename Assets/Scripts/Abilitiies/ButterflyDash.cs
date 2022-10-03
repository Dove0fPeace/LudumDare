using Base_Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        dashTimer.OnTick += UpdateUI;
    }

    private void OnDestroy()
    {
        dashTimer.OnTick -= UpdateUI;
    }

    public void InitiateAbility()
    {
        hud.InitUI(ObjWithCooldown.Ability, null);
    }

    private void UpdateUI()
    {
        hud.UpdateCooldown(ObjWithCooldown.Ability, dashCurrentCooldown);
    }

    public void Use()
    {
        if(!CanDash) return;
        Dash();
    }
}
