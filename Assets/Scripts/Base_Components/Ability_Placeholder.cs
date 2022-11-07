using Base_Components;
using UnityEngine;

public class Ability_Placeholder : MonoBehaviour, IAbility
{
    private Player_HUD hud;

    private void Start()
    {
        if (transform.root.gameObject.CompareTag("Player"))
        {
            hud = Player_HUD.Instance;
        }
        InitiateAbility();
    }
    
    public void Use() { }

    public bool CanUse()
    {
        return false;
    }

    private void InitiateAbility()
    {
        if(hud != null)
            hud.InitUI(ObjWithCooldown.Ability, false, null);
    }
}
