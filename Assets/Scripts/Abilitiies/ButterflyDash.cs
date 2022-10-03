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
    }

    public void InitiateAbility()
    {
        hud.InitUI(ObjWithCooldown.Ability, null);
    }

    protected override void UpdateDashUI()
    {
        base.UpdateDashUI();
        hud.UpdateCooldown(ObjWithCooldown.Ability, dashCurrentCooldown);
    }

    private void OnDestroy()
    {
        hud.InitUI(ObjWithCooldown.Ability, null);
    }
    public void Use()
    {
        if(!CanDash) return;
        Dash();
    }
}
