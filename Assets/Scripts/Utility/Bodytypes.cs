using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyPart
{
    Back,
    Legs,
    Hands,
    Abilitiy
}
[CreateAssetMenu()]
public sealed class Bodytypes : ScriptableObject
{
    public HP_Base[] Backs;
    public Move_Base[] Legs;
    public Attack_Base[] Hands;
    public Ability_Base[] Abilities;
}
