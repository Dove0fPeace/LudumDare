using Base_Components;
using Controls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeDash : Dash_Base, IAbility
{
    public float Damage;

    private List<Unit_Base> enemies = new List<Unit_Base>();

    public bool CanUse()
    {
        return CanDash;
    }

    protected override void Start()
    {
        base.Start();
        InitiateAbility();
        enemies.Add(self);
    }

    private void Update()
    {
        if(DashNow)
        {
            var hit = Physics2D.OverlapPoint(transform.position);
            if(hit)
            {
                var unit = hit.transform.root.GetComponent<Unit_Base>();
                if(unit != null && enemies.Contains(unit) == false)
                {
                    unit.TakeDamage(Damage, false);
                    enemies.Add(hit.transform.root.GetComponent<Unit_Base>());
                }

            }
        }
    }

    public void InitiateAbility()
    {       
        if (ai != null) return;
        hud.InitUI(ObjWithCooldown.Ability, true, dashTimer);
    }

    protected override IEnumerator Dashing()
    {
        self.SetInvincible(InvincibleInDash);
        DashNow = true;
        Move_Target.SetLayer(LayerMask.NameToLayer("bugDash"));
        Move_Target.IsCanMove = false;
        CanDash = false;
        yield return new WaitForSeconds(DashMoveBlock);
        rb.Sleep();
        Move_Target.IsCanMove = true;
        yield return new WaitForSeconds(DashCooldown - DashMoveBlock);
        Move_Target.SetLayer(LayerMask.NameToLayer("bug"));
        self.SetInvincible(false);
        DashNow = false;
        CanDash = true;
        //hud.InitUI(ObjWithCooldown.Dash);
        dashTimer.Restart(true);
        enemies.Clear();
        enemies.Add(self);
    }
    public void Use()
    {
        if (!CanDash) return;
        Dash();
    }

}
