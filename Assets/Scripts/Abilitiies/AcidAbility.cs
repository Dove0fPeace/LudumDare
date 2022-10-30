using Base_Components;
using Controls;
using UnityEngine;

public class AcidAbility : RangeAttack_Base, IAbility
{
    public Insects Insect = Insects.Scorpion;

    [Header("Ability Settings")]
    public Sprite AbilityUISprite;
    public float AbilityCooldown;

    private Timer timer;
    private float currentCooldown;
    private bool CooldownIsDown = true;

    protected override void Start()
    {
        base.Start();
        InitiateAbility();
    }

    public bool CanUse()
    {
        return CanAttack;
    }

    public void InitiateAbility()
    {
        var ai = transform.root.GetComponent<AI>();
        timer = Timer.CreateTimer(AbilityCooldown,false);
        timer.Pause();
        if (ai == null)
        {
            hud.InitUI(ObjWithCooldown.Ability, true, Timer_AttackCooldown);
            timer.OnTick += OnCDTick;
        }
        timer.OnTimeRunOut += OnCDComplete;
    }

    private void OnCDTick()
    {
        currentCooldown = timer.CurrentTime / AbilityCooldown;
    }

    private void OnCDComplete()
    {
        CooldownIsDown = true;
        timer.Restart(true);
        var ai = transform.root.GetComponent<AI>();
        if (ai != null) return;
    }

    public void Use()
    {
        if (!CooldownIsDown) return;
        Attack();
        CooldownIsDown = false;
        timer.Play();
    }

    private void OnDestroy()
    {
        timer.OnTick -= OnCDTick;
        timer.OnTimeRunOut -= OnCDComplete;
        timer.Destroy();
    }

}
