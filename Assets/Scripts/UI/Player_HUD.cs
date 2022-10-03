using System.Collections;
using System.Collections.Generic;
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
