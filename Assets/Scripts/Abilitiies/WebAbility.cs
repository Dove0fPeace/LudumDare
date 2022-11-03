using Base_Components;
using Controls;
using UnityEngine;

public class WebAbility : RangeAttack_Base, IAbility
{
    public Insects Insect => Insects.Spider;

    private AI ai;

    [Header("Ability Settings")]
    public Sprite AbilityUISprite;
    public float AbilityCooldown;

    private Timer timer;
    private bool CooldownIsDown = true;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        self = transform.root.GetComponent<Unit_Base>();
        HandsPlace = self.HandsPosition;
        if (transform.root.gameObject.CompareTag("Player"))
        {
            hud = Player_HUD.Instance;
        }
        InitiateAbility();
        CanAttack = true;
    }
    
    public bool CanUse()
    {
        return CanAttack;
    }

    public void InitiateAbility()
    {
        Timer_AttackCooldown = Timer.CreateTimer(AttackCooldown, false, false);
        timer = Timer.CreateTimer(AbilityCooldown, false, false);
        timer.Pause();
        if(hud != null)
        {
            hud.InitUI(ObjWithCooldown.Ability, true, timer);
        }
        timer.OnTimeRunOut += OnCDComplete;
    }

    private void OnCDComplete()
    {
        CooldownIsDown = true;
        timer.Restart(true);
    }

    public void Use()
    {
        if (!CooldownIsDown) return;
        Attack();
        CooldownIsDown = false;
        timer.Play();
    }

    protected override void OnDestroy()
    {
        timer.OnTimeRunOut -= OnCDComplete;
        timer.Destroy();
    }
}
