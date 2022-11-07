using UnityEngine;

public class Attack_Base : MonoBehaviour
{
    public bool EnableAttack;
    public float AttackRange = 30f;
    public float Damage;
    public AudioClip sfx;

    public float AttackCooldown;
    public Timer Timer_AttackCooldown;
    public bool CanAttack;

    public Transform HandsPlace;
    public virtual Insects InsectType => Insects.Generic;
    public ObjWithCooldown AttackType;

    protected Unit_Base self;
    public Player_HUD hud;

    protected virtual void Start()
    {
        self = transform.root.GetComponent<Unit_Base>();
        Timer_AttackCooldown = Timer.CreateTimer(AttackCooldown, false, false);
        Timer_AttackCooldown.OnTimeRunOut += OnAttackCooldownComplete;
        if (transform.root.gameObject.CompareTag("Player"))
        {
            hud = Player_HUD.Instance;
            hud.InitUI(AttackType, EnableAttack, Timer_AttackCooldown);
        }
        CanAttack = true;
    }

    protected virtual void OnDestroy()
    {
        if(Timer_AttackCooldown != null)
        {
            Timer_AttackCooldown.OnTimeRunOut -= OnAttackCooldownComplete;
            Timer_AttackCooldown.Destroy();
        }
    }

    public virtual bool Attack()
    {
        if (CanAttack == false || EnableAttack == false)
        {
            return false;
        }
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
