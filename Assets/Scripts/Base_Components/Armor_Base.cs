using UnityEngine;

public class Armor_Base : MonoBehaviour
{
    public bool invincible;
    public float PhysicalResist = 1f;
    [SerializeField] private GameObject vfx;
    [SerializeField] private AudioClip sfx;
    public virtual Insects InsectType => Insects.Generic;

    private Unit_Base parent;

    private void Start()
    {
        //we can init it upon generating the component with the right reference 
        parent = transform.root.GetComponent<Unit_Base>();
    }

    public float CalculateDamage(float initialDamage)
    {
        if (invincible || initialDamage < 0.1f)
        {
            return 0;
        }
        
        PlayDamageEffect();
        return initialDamage * PhysicalResist;
    }

    private void PlayDamageEffect()
    {
        Instantiate(vfx, transform.root.position, Quaternion.Euler(0, 90, 0));
        if(sfx!= null)
        { 
            parent.PlayAudioOneshot(sfx);
        }
    }
}
