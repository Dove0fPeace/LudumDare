using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Armor_Base : MonoBehaviour
{
    public BodyPart Part = BodyPart.Back;

    public float PhysicalResist = 1f;
    [SerializeField] private GameObject vfx;
    [SerializeField] private AudioClip sfx;

    private Unit_Base parent;

    private void Start()
    {
        parent = transform.root.GetComponent<Unit_Base>();
    }

    public void PlayDamageEffect()
    {
        Instantiate(vfx, transform.root.position, Quaternion.Euler(0, 90, 0));
        if(sfx!= null)
        { 
            parent.PlayAudioOneshot(sfx);
        }
    }
}
