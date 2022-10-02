using UnityEngine;

public enum BodyPart
{
    Back,
    Legs,
    Hands,
    Abilitiy
}

public enum Insects
{
    Scarabei,
    Spider,
    Scorpion,
    Moth,
    Generic
}

[CreateAssetMenu()]
public sealed class Bodytypes : ScriptableObject
{
    public GameObject[] Backs;
    public GameObject[] Fronts;
}
