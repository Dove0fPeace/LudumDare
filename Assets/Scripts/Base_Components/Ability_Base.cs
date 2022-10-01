using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Base : MonoBehaviour
{

    private void Update()
    {
        if(Input.GetAxisRaw("Fire2") != 0)
        {
            UseAbility();
        }
    }
    public virtual void UseAbility()
    {
        print("Use ability");
    }
}
