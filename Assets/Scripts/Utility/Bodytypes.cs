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
    public GameObject[] Backs;
    public GameObject[] Fronts;
}
