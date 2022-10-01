using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bite : Attack_Base
{
    public float BiteRange;
    public float BiteSpeed;

    private bool damagePhase = false;

    public override void Attack()
    {
        base.Attack();
        ChangeDamagePhase();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!damagePhase) return;
        var enemy = collision.otherCollider.transform.GetComponent<Unit_Base>();
        if(enemy != null)
        {
            enemy.TakeDamage(Damage);
        }
    }

    private void ChangeDamagePhase()
    {
        damagePhase = !damagePhase;
    }
}
