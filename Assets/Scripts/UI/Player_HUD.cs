using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum ObjWithCooldown
{
    Dash,
    Ability
}

public class Player_HUD : SingletonBase<Player_HUD>
{
    [Header("Dash")]
    public Image DashIconOverlay;

    [Header("Ability")]
    public Image AbilityIconOverlay;

    [Header("AttackPossibility")] public Image[] AttackIcons;
    private Unit_Base Player;
    private bool attackDisabled;

    public void InitUI(ObjWithCooldown obj, Sprite sprite)
    {
        switch (obj)
        {
            case ObjWithCooldown.Dash:
                DashIconOverlay.fillAmount = 1;
                break;
            case ObjWithCooldown.Ability:
                AbilityIconOverlay.fillAmount = 1;
                break;
        }
    }

    public void ChangeAttack(bool unable)
    {
        if (!attackDisabled && unable)
        {
            attackDisabled = true;
            foreach (Image attackIcon in AttackIcons)
            {
                attackIcon.DOColor(Color.gray, 1f);
            }
        }
        else if (attackDisabled && !unable)
        {
            attackDisabled = false;
            foreach (Image attackIcon in AttackIcons)
            {
                attackIcon.DOColor(Color.white, 1f);
            }
        }
    }

    public void UpdateCooldown(ObjWithCooldown obj, float percent)
    {
        switch(obj)
        { 
            case ObjWithCooldown.Dash:
                DashIconOverlay.fillAmount = (1-percent);
                break;
            case ObjWithCooldown.Ability:
                AbilityIconOverlay.fillAmount = (1-percent);
                break;
        }
    }
}
