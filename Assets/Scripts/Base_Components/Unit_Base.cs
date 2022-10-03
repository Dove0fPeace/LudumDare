using Base_Components;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Base : MonoBehaviour
{
    [Header("HP")]
    public int MaxHitPoints;
    [HideInInspector]public float CurrentHP;
    public Slider HpBar;
    private RectTransform fillRect;
    public Color DefaultHPColor;

    [Space(5)]

    public Bodytypes Bodytypes;

    public Armor_Base Armor;
    public Move_Base Move;
    public Attack_Base Attack;
    public IAbility Ability;

    [Header("BodyPosition")] 
    public Transform RotationRoot;
    public Transform HandsPosition;
    public Transform FrontPosition;
    public Transform BackPosition;

    private GameObject front;
    private Animator frontAnim;
    private GameObject back;
    private Animator backAnim;
    
    private AudioSource AudioSource;
    private Vector3 initialPosition;
    private bool invincible;


    private void Start()
    {
        ChangeBody();
        CurrentHP = MaxHitPoints;
        HpBar.maxValue = MaxHitPoints;
        HpBar.value = HpBar.maxValue;
        initialPosition = transform.position;
        AudioSource = GetComponent<AudioSource>();

        fillRect = HpBar.fillRect;

        GameLoop.Instance.AddToUnitList(this);
    }

    public void SetGodMode(bool on = true)
    {
        invincible = on;
    }

    private void Update()
    {
        HpBar.value = CurrentHP;
    }

    public void ChangeBody()
    {
        if (!gameObject.activeInHierarchy)
        {
            //respawn
            transform.position = initialPosition;
            CurrentHP = MaxHitPoints;
            HpBar.maxValue = MaxHitPoints;
            HpBar.value = HpBar.maxValue;
            gameObject.SetActive(true);
        } 
        Clear();

        front = Instantiate(Bodytypes.GetRandomFront(), FrontPosition, false);
        front.transform.localPosition = Vector3.zero;
        frontAnim = front.GetComponent<Animator>();
        back = Instantiate(Bodytypes.GetRandomBack(), BackPosition, false);
        back.transform.localPosition = Vector3.zero;
        backAnim = back.GetComponent<Animator>();
        
        //priorities: move and attack from front, armor and ability from back
        Armor = front.GetComponentInChildren<Armor_Base>();
        Armor = back.GetComponentInChildren<Armor_Base>();
        Move = back.GetComponentInChildren<Move_Base>();
        Move = front.GetComponentInChildren<Move_Base>();
        Attack = back.GetComponentInChildren<Attack_Base>();
        Attack = front.GetComponentInChildren<Attack_Base>();
        Ability = front.GetComponentInChildren<IAbility>();
        Ability = back.GetComponentInChildren<IAbility>();
        
        //init components
        Armor.invincible = invincible;
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

    private void PlayAnimBool(string anim, bool on)
    {
        frontAnim.SetBool(anim, on);
        backAnim.SetBool(anim, on);
    }
    
    private void PlayAnim(string anim)
    {
        frontAnim.SetTrigger(anim);
        backAnim.SetTrigger(anim);
    }

    public bool TryMove(Vector2 direction)
    {
        if (Move is null || !Move.IsCanMove)
        {
            return false;
        }
        Move.SetMove(direction.x, direction.y);
        PlayAnimBool("Move", true);
        return true;
    }

    public void StopAnimation(string stateName)
    {
        PlayAnimBool("Move", false);
    }

    public bool TryDash()
    {
        if (Move is null || !Move.IsCanMove)
        {
            return false;
        }
        Move.Dash();
        PlayAnimBool("Move", true);
        return true;
    }

    public bool TryAttack()
    {
        if (Attack is null || Attack.CanAttack == false)
        {
            return false;
        }
        Attack.Attack();
        PlayAnim("Attack");
        return true;
    }

    public bool TryAbility()
    {
        if (Ability is null)
        {
            return false;
        }
        Ability.Use();
        PlayAnim("Ability");
        return true;
    }

    public void LookAt(Vector2 targetLookPos)
    {
        RotationRoot.right = targetLookPos - new Vector2 (transform.position.x, transform.position.y);
    }

    public void TakeDamage(float damage, bool isPoison)
    {
        print("Take damage " + damage);
        if(!isPoison)
        {
            CurrentHP -= Armor.CalculateDamage(damage);
        }
        else
        {
            CurrentHP -= damage;
        }
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
    
    public void ChangeHPBarColor(Color color)
    {
        fillRect.GetComponent<Image>().color = color;
    }
    public void Death()
    {
        gameObject.SetActive(false);
    }
}
