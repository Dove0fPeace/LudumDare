using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Base : MonoBehaviour
{
    public BodyPart Part = BodyPart.Hands;

    public float AttackRange = 30f;
    public int Damage;

    public float AttackCooldown;
    private float attackTime = 0;
    public bool CanAttack => attackTime <= 0;

    public Transform HandsPlace;

    [Header("Animation")]
    public Animator animator;
    public string AttackAnimationName;

    protected Unit_Base self;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        self = transform.root.GetComponent<Unit_Base>();
    }
    private void Update()
    {
        if(attackTime > 0)
            attackTime -= Time.deltaTime;
    }

    public virtual void Attack()
    {
        if(CanAttack == false) return;
        //animator.Play(AttackAnimationName, 0, 0f);
        attackTime = AttackCooldown;
    }
}
