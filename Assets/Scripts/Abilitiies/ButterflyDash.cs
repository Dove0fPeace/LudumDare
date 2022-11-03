using Base_Components;
using Controls;
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
        ai = transform.root.GetComponent<AI>();
        if (ai != null) return;
        InitiateAbility();
    }

    public void InitiateAbility()
    {
        var ai = transform.root.GetComponent<AI>();
        if (ai != null) return;
        hud.InitUI(ObjWithCooldown.Ability, true, dashTimer);
    }
    public void Use()
    {
        if(!CanDash) return;
        Dash();
    }
}
