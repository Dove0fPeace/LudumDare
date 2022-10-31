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
    public AudioSource AudioSource;
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
    public float DuratiomScaleUI;
    public AudioClip FailedUseSound;

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
                if(active)
                {
                    DashBlockIcon.enabled = false;
                }
                else
                {
                    DashBlockIcon.enabled = true;
                }
                DashIsActive = active;
                DashIconOverlay.fillAmount = 1;
                DashCooldown = cooldownTimer;
                break;
            case ObjWithCooldown.Ability:
                if (active)
                {
                    AbilityBlockIcon.enabled = false;
                }
                else
                {
                    AbilityBlockIcon.enabled = true;
                }
                AbilityIsActive = active;
                AbilityIconOverlay.fillAmount = 1;
                AbilityCooldown = cooldownTimer;
                break;
            case ObjWithCooldown.Attack:
                if (active)
                {
                    AttackBlockIcon.enabled = false;
                }
                else
                {
                    AttackBlockIcon.enabled = true;
                }
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
                DashIcon.DOPunchScale(ScaleUIElement, DuratiomScaleUI);
                break;
            case ObjWithCooldown.Ability:
                AbilityIcon.DOPunchScale(ScaleUIElement, DuratiomScaleUI);
                break;
            case ObjWithCooldown.Attack:
                AttackIcon.DOPunchScale(ScaleUIElement, DuratiomScaleUI);
                break;
        }
        if(FailedUseSound != null)
            AudioSource.PlayOneShot(FailedUseSound);
    }

}
