using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Base : MonoBehaviour
{
    public BodyPart Part = BodyPart.Abilitiy;

    public virtual void UseAbility()
    {
        print("Use ability");
    }
}
