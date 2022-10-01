using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Base : MonoBehaviour
{
    public BodyPart Part = BodyPart.Hands;

    public int Damage;

    public float AttackCooldown;
    private float attackTime = 0;
    public bool CanAttack => attackTime <= 0;

    public Transform HandsPlace;

    protected Unit_Base self;

    protected virtual void Start()
    {
        print("Start");
        self = transform.root.GetComponent<Unit_Base>();
        print(self);
    }
    private void Update()
    {
        if(attackTime > 0)
            attackTime -= Time.deltaTime;
    }

    public virtual void Attack()
    {
        if(CanAttack == false) return;
        attackTime = AttackCooldown;
    }
}
