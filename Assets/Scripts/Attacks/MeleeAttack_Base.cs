using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

public class MeleeAttack_Base : Attack_Base
{
    [Header("Melee Attack settings")]
    public float AttackRange;
    public float AttackCastRadius;
    public float DamageDelay;

    public override void Attack()
    {
        print("Attack");
        base.Attack();
        StartCoroutine(CheckRaycastOnAttack());
    }

    public IEnumerator CheckRaycastOnAttack()
    {
        yield return new WaitForSeconds(DamageDelay);

        RaycastHit2D hit;
        hit = Physics2D.CircleCast(transform.position, AttackCastRadius, transform.right, AttackRange);
        if(hit)
        {
            Unit_Base enemy = hit.collider.transform.root.GetComponent<Unit_Base>();
            if(enemy != null && enemy != self)
            {
                print(enemy.name);
                enemy.TakeDamage(Damage);
            }
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackCastRadius);
        Gizmos.DrawLine(transform.position, transform.position + (transform.right * (AttackRange + AttackCastRadius)));
    }
#endif

}
