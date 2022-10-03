using System;
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
    public Image DashIcon;
    public Image DashIconOverlay;

    [Header("Ability")]
    public Image AbilityIcon;
    public Image AbilityIconOverlay;

    [Header("AttackPossibility")] public Image[] AttackIcons;
    private Unit_Base Player;
    private bool attackDisabled;

    public void InitUI(ObjWithCooldown obj, Sprite sprite)
    {
        switch (obj)
        {
            case ObjWithCooldown.Dash:
                DashIconOverlay.fillAmount = 0;
                break;
            case ObjWithCooldown.Ability:
                //AbilityIcon.sprite = sprite;
                AbilityIconOverlay.fillAmount = 0;
                break;
        }
    }

    protected override void Awake()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Unit_Base>();
    }

    private void Update()
    {
        if (!attackDisabled && Player.Attack.UnableToAttack)
        {
            attackDisabled = true;
            foreach (Image attackIcon in AttackIcons)
            {
                attackIcon.DOColor(Color.gray, 1f);
            }
        }
        else if (attackDisabled && !Player.Attack.UnableToAttack)
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
                DashIconOverlay.fillAmount = percent;
                break;
            case ObjWithCooldown.Ability:
                AbilityIconOverlay.fillAmount = percent;
                break;
        }

    }
}
