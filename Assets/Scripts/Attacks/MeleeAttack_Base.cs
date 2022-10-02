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
            print("Hit");
            Unit_Base enemy = hit.collider.transform.root.GetComponent<Unit_Base>();
            if(enemy != null)
            {
                print("Enemy hit");
                OnEnemyHit(enemy);
            }
        }
    }

    public virtual void OnEnemyHit(Unit_Base unit)
    {
        unit.TakeDamage(Damage,false);
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
