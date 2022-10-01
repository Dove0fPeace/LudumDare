using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Base : MonoBehaviour
{
    public int Damage;

    private void Update()
    {
        if(Input.GetAxisRaw("Fire1") != 0)
        {
            Attack();
        }
    }
    public virtual void Attack()
    {
        print("Atack");
    }
}
