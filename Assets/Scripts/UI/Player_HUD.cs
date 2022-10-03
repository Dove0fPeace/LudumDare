using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ObjWithCooldown
{
    Dash,
    Ability
}

public class Player_HUD : SingletonBase<Player_HUD>
{
    [Header("Dash")]
    public Image DashIcon;
    public TMP_Text DashCooldownCounter;

    [Header("Ability")]
    public Image AbilityIcon;
    public TMP_Text AbilityCooldownCounter;

    public void InitUI(ObjWithCooldown obj, Sprite sprite)
    {
        switch (obj)
        {
            case ObjWithCooldown.Dash:
                //DashIcon.sprite = sprite;
                DashCooldownCounter.text = "";
                break;
            case ObjWithCooldown.Ability:
                //AbilityIcon.sprite = sprite;
                AbilityCooldownCounter.text = "";
                break;
        }
    }

    public void UpdateCooldown(ObjWithCooldown obj, int cooldown)
    {
        switch(obj)
        { 
            case ObjWithCooldown.Dash:
                DashCooldownCounter.text =(1+ cooldown).ToString();
                break;
            case ObjWithCooldown.Ability:
                AbilityCooldownCounter.text = (1 + cooldown).ToString();
                break;
        }

    }
}
