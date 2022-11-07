using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum ObjWithCooldown
{
    Dash,
    Ability,
    Attack
}

public class Player_HUD : SingletonBase<Player_HUD>
{
    private AudioSource audioSource;
    [Header("Dash")]
    public Image DashIconOverlay;
    public Image DashBlockIcon;
    public RectTransform DashIcon;
    private Timer DashCooldown;
    private bool DashIsActive;

    [Header("Ability")]
    public Image AbilityIconOverlay;
    public Image AbilityBlockIcon;
    public RectTransform AbilityIcon;
    private Timer AbilityCooldown;
    private bool AbilityIsActive;

    [Header("Attack")]
    public Image AttackIconOverlay;
    public Image AttackBlockIcon;
    public RectTransform AttackIcon;
    private Timer AttackCooldown;
    private bool AttackIsActive;

    [Header("Try use anything on cooldown")]
    public Vector3 ScaleUIElement;
    public float DurationScaleUI;
    public AudioClip FailedUseSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        DashIsActive = false;
        AttackIsActive = false;
        AbilityIsActive = false;
    }

    private void Update()
    {
        if(DashIsActive && DashCooldown != null)
            DashIconOverlay.fillAmount = DashCooldown.CurrentTime/DashCooldown.MaxTime;
        if(AbilityIsActive && AbilityCooldown != null)
            AbilityIconOverlay.fillAmount = AbilityCooldown.CurrentTime/AbilityCooldown.MaxTime;
        if(AttackIsActive && AttackCooldown != null)
            AttackIconOverlay.fillAmount = AttackCooldown.CurrentTime/AttackCooldown.MaxTime;
    }

    public void InitUI(ObjWithCooldown obj, bool active ,Timer cooldownTimer)
    {
        switch (obj)
        {
            case ObjWithCooldown.Dash:
                DashBlockIcon.enabled = !active;
                DashIsActive = active;
                DashIconOverlay.fillAmount = 1;
                DashCooldown = cooldownTimer;
                break;
            case ObjWithCooldown.Ability:
                AbilityBlockIcon.enabled = !active;
                AbilityIsActive = active;
                if(cooldownTimer == null)
                {
                    AbilityCooldown = null;
                }
                AbilityCooldown = cooldownTimer;
                AbilityIconOverlay.fillAmount = 1;
                break;
            case ObjWithCooldown.Attack:
                AttackBlockIcon.enabled = !active;
                AttackIsActive = active;
                AttackIconOverlay.fillAmount = 1;
                AttackCooldown = cooldownTimer;
                break;
        }
    }

    public void TryUseOnCooldown(ObjWithCooldown obj)
    {
        switch (obj)
        {
            case ObjWithCooldown.Dash:
                DashIcon.DOPunchScale(ScaleUIElement, DurationScaleUI);
                break;
            case ObjWithCooldown.Ability:
                AbilityIcon.DOPunchScale(ScaleUIElement, DurationScaleUI);
                break;
            case ObjWithCooldown.Attack:
                AttackIcon.DOPunchScale(ScaleUIElement, DurationScaleUI);
                break;
        }
        if(FailedUseSound != null)
            audioSource.PlayOneShot(FailedUseSound);
    }

}
