using System.Collections;
using UnityEngine;

public class MeleeAttack : Attack_Base
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

        RaycastHit2D[] hit;
        hit = Physics2D.CircleCastAll(transform.position, AttackCastRadius, transform.right, AttackRange);

        foreach (RaycastHit2D hit2 in hit)
        {
            Unit_Base enemy =  hit2.transform.root.GetComponent<Unit_Base>();
            if(enemy != null && enemy != self)
            {
                enemy.TakeDamage(Damage,false);
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
