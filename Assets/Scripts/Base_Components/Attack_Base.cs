using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Base : MonoBehaviour
{
    public bool UnableToAttack;
    public float AttackRange = 30f;
    public float Damage;
    public AudioClip sfx;

    public float AttackCooldown;
    public float attackTime = 0;
    public bool CanAttack => attackTime <= 0 && !UnableToAttack;

    public Transform HandsPlace;
    public virtual Insects InsectType => Insects.Generic;

    protected Unit_Base self;

    protected virtual void Start()
    {
        self = transform.root.GetComponent<Unit_Base>();
        if (transform.root.gameObject.CompareTag("Player"))
        {
            Player_HUD.Instance.ChangeAttack(UnableToAttack);
        }
    }
    protected virtual void Update()
    {
        if(attackTime > 0)
            attackTime -= Time.deltaTime;
    }

    public virtual bool Attack()
    {
        if(CanAttack == false) return false;
        attackTime = AttackCooldown;
        if (sfx)
        {
            self.PlayAudioOneshot(sfx);
        }

        return true;
    }
}
