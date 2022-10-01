using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Base : MonoBehaviour
{
    public BodyPart Part = BodyPart.Hands;

    public int Damage;

    public virtual void Attack()
    {
        print("Atack");
    }
}
