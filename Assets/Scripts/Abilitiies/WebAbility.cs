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
    private float currentCooldown;
    private bool CooldownIsDown = true;

    private Player_HUD hud;

    protected override void Start()
    {
        base.Start();
        ai = transform.root.GetComponent<AI>();
        InitiateAbility();
    }
    
    public bool CanUse()
    {
        return CanAttack;
    }

    public void InitiateAbility()
    {
        timer = Timer.CreateTimer(AbilityCooldown, false);
        timer.Pause();
        hud = Player_HUD.Instance;
        hud.InitUI(ObjWithCooldown.Ability);
        timer.OnTick += OnCDTick;
        timer.OnTimeRunOut += OnCDComplete;
    }

    private void OnCDTick()
    {
        if (ai != null) return;
        currentCooldown = timer.CurrentTime / AbilityCooldown;
        hud.UpdateCooldown(ObjWithCooldown.Ability, currentCooldown);
    }

    private void OnCDComplete()
    {
        CooldownIsDown = true;
        timer.Restart();
        timer.Pause();
        if (ai != null) return;
        hud.InitUI(ObjWithCooldown.Ability);
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
        hud.InitUI(ObjWithCooldown.Ability);
    }
}
