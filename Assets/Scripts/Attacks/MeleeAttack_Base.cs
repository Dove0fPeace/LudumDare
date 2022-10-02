using System.Collections;
using UnityEngine;

public class MeleeAttack_Base : Attack_Base
{
    [Header("Melee Attack settings")]
    public float AttackCastRadius;
    public float DamageDelay;

    public override bool Attack()
    {
        if (base.Attack())
        {
            print("Attack");
            StartCoroutine(CheckRaycastOnAttack());
            return true;
        }

        return false;
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
