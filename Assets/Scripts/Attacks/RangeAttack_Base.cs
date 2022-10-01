using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack_Base : Attack_Base
{
    public Projectile_Base ProjectilePrefab;

    public override void Attack()
    {
        base.Attack();
        var projectile = Instantiate(ProjectilePrefab, HandsPlace.position, HandsPlace.rotation);
        projectile.Spawn(self, Damage);
    }
}
