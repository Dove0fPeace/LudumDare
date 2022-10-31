using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Base : MonoBehaviour
{
    public BodyPart Part = BodyPart.Hands;

    public bool EnableAttack;
    public float AttackRange = 30f;
    public float Damage;
    public AudioClip sfx;

    public float AttackCooldown;
    public Timer Timer_AttackCooldown;
    public bool CanAttack;

    public Transform HandsPlace;

    [Header("Animation")]
    public Animator animator;
    public string AttackAnimationName;
    public virtual Insects InsectType => Insects.Generic;

    protected Unit_Base self;
    public Player_HUD hud;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        self = transform.root.GetComponent<Unit_Base>();
        Timer_AttackCooldown = Timer.CreateTimer(AttackCooldown, false, false);
        Timer_AttackCooldown.OnTimeRunOut += OnAttackCooldownComplete;
        if (transform.root.gameObject.CompareTag("Player"))
        {
            hud = Player_HUD.Instance;
            hud.InitUI(ObjWithCooldown.Attack, EnableAttack, Timer_AttackCooldown);
        }
        CanAttack = true;
    }

    private void OnDestroy()
    {
        Timer_AttackCooldown.OnTimeRunOut -= OnAttackCooldownComplete;
        if(Timer_AttackCooldown != null)
            Timer_AttackCooldown.Destroy();
    }

    public virtual bool Attack()
    {
        if (CanAttack == false || EnableAttack == false)
        {
            if(hud != null)
                hud.TryUseOnCooldown(ObjWithCooldown.Attack);
            return false;
        }
            //animator.Play(AttackAnimationName, 0, 0f);
        Timer_AttackCooldown.Play();
        CanAttack = false;
        if (sfx)
        {
            self.PlayAudioOneshot(sfx);
        }

        return true;
    }

    private void OnAttackCooldownComplete()
    {
        Timer_AttackCooldown.Restart(true);
        CanAttack = true;
    }
}
