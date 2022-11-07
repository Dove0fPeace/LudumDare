using Base_Components;
using Controls;
using UnityEngine;

public class WebAbility : RangeAttack, IAbility
{
    public Insects Insect => Insects.Spider;

    public bool CanUse()
    {
        return CanAttack;
    }

    public void Use()
    {
        Attack();
    }
}
