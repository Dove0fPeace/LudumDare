using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bite : Attack_Base
{
    public Animator animator;

    private bool damagePhase = false;
    private Unit_Base self;

    private void Start()
    {
        animator = GetComponent<Animator>();
        self = transform.root.GetComponent<Unit_Base>();
    }
    public override void Attack()
    {
        base.Attack();
        damagePhase = true;
        animator.Play("Bite",0,0f);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        var enemy = collision.transform.GetComponent<Unit_Base>();
        if (enemy != null && damagePhase)
        {
            damagePhase = false;
            enemy.TakeDamage(Damage);
        }
    }

    private void DamagePhaseEnd()
    {
        damagePhase = false;
    }
}
