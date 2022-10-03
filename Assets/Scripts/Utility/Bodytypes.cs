using System.Linq;
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
    public bool MixParts;

    public GameObject GetRandomFront()
    {
        if (MixParts)
        {
            return RandomPart();
        }

        {
            return Fronts[Random.Range(0, Fronts.Length)];
        }
    }
    
    public GameObject GetRandomBack()
    {
        if (MixParts)
        {
            return RandomPart();
        }

        {
            return Backs[Random.Range(0, Fronts.Length)];
        }
    }

    private GameObject RandomPart()
    {
        var allParts = Backs.Union(Fronts).ToArray();
        return allParts[Random.Range(0, allParts.Length)];
    }
}
