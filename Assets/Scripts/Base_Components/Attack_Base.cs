using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Base : MonoBehaviour
{
    public BodyPart Part = BodyPart.Hands;

    public int Damage;

    public float AttackCooldown;
    private float attackTime = 0;
    private bool CanAttack => attackTime <= 0;

    public Transform HandsPlace;

    private void Update()
    {
        if(attackTime > 0)
            attackTime -= Time.deltaTime;
    }

    public virtual void Attack()
    {
        if(!CanAttack) return;
        attackTime = AttackCooldown;
    }
}
