using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

public class MeleeAttack_Base : Attack_Base
{
    [Header("Animation")]
    public Animator animator;
    public string AttackAnimationName;

    [Header("Melee Attack settings")]
    public float AttackRange;
    public float AttackCastRadius;
    public float DamageDelay;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }
    public override void Attack()
    {
        base.Attack();
        //animator.Play(AttackAnimationName, 0,0f);
        StartCoroutine(CheckRaycastOnAttack());
    }

    public IEnumerator CheckRaycastOnAttack()
    {
        yield return new WaitForSeconds(DamageDelay);

        RaycastHit2D hit;
        hit = Physics2D.CircleCast(transform.position, AttackCastRadius, transform.up, AttackRange);
        if(hit)
        {
            Unit_Base enemy = hit.collider.transform.root.GetComponent<Unit_Base>();
            if(enemy != null && enemy != self)
            {
                enemy.TakeDamage(Damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackCastRadius);
        Gizmos.DrawLine(transform.position, transform.position + (transform.up * (AttackRange + AttackCastRadius)));
    }

}
