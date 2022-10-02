using Base_Components;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class Unit_Base : MonoBehaviour
{
    [Header("HP")]
    public int MaxHitPoints;
    [HideInInspector]public int CurrentHP;
    public Slider HpBar;

    [Space(5)]

    public Bodytypes Bodytypes;

    private Armor_Base Armor;
    private Move_Base Move;
    private Attack_Base Attack;
    private IAbility Ability;

    [Header("BodyPosition")]
    public Transform HandsPosition;
    public Transform BackPosition;

    private GameObject front;
    private GameObject back;
    
    public Vector2 targetLookPos;

    private AudioSource AudioSource;

    private void Start()
    {
        ChangeBody();
        CurrentHP = MaxHitPoints;
        HpBar.maxValue = MaxHitPoints;
        HpBar.value = HpBar.maxValue;
    }

    private void Update()
    {
        HpBar.value = CurrentHP;
    }

    public void ChangeBody()
    {
        Clear();

        front = Instantiate(Bodytypes.Fronts[Random.Range(0, Bodytypes.Fronts.Length)],
            HandsPosition.position,
            HandsPosition.rotation, transform);
        back = Instantiate(Bodytypes.Backs[Random.Range(0, Bodytypes.Backs.Length)],
            BackPosition.position,
            BackPosition.rotation, transform);
        Armor = back.GetComponentInChildren<Armor_Base>();
        Move = front.GetComponentInChildren<Move_Base>();
        Attack = front.GetComponentInChildren<Attack_Base>();
        Attack.HandsPlace = HandsPosition;
        Ability = back.GetComponentInChildren<IAbility>();
    }

    public void PlayAudioOneshot(AudioClip clip)
    {
        AudioSource.PlayOneShot(clip);
    }

    private void Clear()
    {
        if (front)
        {
            Destroy(front);
        }

        if (back)
        {
            Destroy(back);
        }
    }

    public bool TryMove(Vector2 direction)
    {
        if (Move is null || !Move.IsCanMove)
        {
            return false;
        }
        Move.SetMove(direction.x, direction.y);
        return true;
    }

    public bool TryDash()
    {
        if (Move is null || !Move.IsCanMove)
        {
            return false;
        }
        Move.Dash();
        return true;
    }

    public bool TryAttack()
    {
        if (Attack is null || Attack.CanAttack == false)
        {
            return false;
        }
        
        Attack.Attack();
        return true;
    }

    public bool TryAbility()
    {
        if (Ability is null)
        {
            return false;
        }
        Ability.Use();
        return true;
    }

    public void LookAt(Vector2 targetLookPos)
    {
        transform.up = targetLookPos - new Vector2 (transform.position.x, transform.position.y);
    }

    public void TakeDamage(int damage)
    {
        print("Take damage " + damage);
        CurrentHP -= (int)(damage * Armor.PhysicalResist);

         if(CurrentHP <= 0)
         {
            Death();
         }
    }

    public void Heal(int heal)
    {
        CurrentHP += heal;
        if(CurrentHP > MaxHitPoints)
        {
            CurrentHP = MaxHitPoints;
        }
    }

    public void Heal(float healPercent)
    {
        CurrentHP += (int)(MaxHitPoints * healPercent);
        if(CurrentHP > MaxHitPoints)
        {
            CurrentHP = MaxHitPoints;
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}