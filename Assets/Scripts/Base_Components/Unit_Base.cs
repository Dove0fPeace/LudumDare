using Base_Components;
using UnityEngine;
using UnityEngine.UI;
using Controls;

public class Unit_Base : MonoBehaviour
{
    [Header("HP")]
    public int MaxHitPoints;
    public float CurrentHP;
    public Slider HpBar;
    private RectTransform fillRect;
    public Color DefaultHPColor;
    public Canvas canvas;

    [Space(5)]

    public Bodytypes Bodytypes;

    public Armor_Base Armor;
    public Move_Base Move;
    public Dash_Base Dash;
    public Attack_Base Attack;
    public IAbility Ability;

    [Header("BodyPosition")] 
    public Transform RotationRoot;
    public Transform HandsPosition;
    public Transform FrontPosition;
    public Transform BackPosition;

    private GameObject front;
    private Animator[] frontAnims;
    private GameObject back;
    private Animator backAnim;
    
    private AudioSource AudioSource;
    private Vector3 initialPosition;
    private bool invincible;

    public Kokon KokonPrefab;
    private Control_Base control;

    private Vector2 targetLookAt;
    private float rotateSpeed = 5f;


    private void Start()
    {
        control = transform.GetComponent<Control_Base>();
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

        float angle = Vector2.SignedAngle(RotationRoot.right, targetLookAt);
        if (Mathf.Abs(angle) > rotateSpeed)
        {
            RotationRoot.rotation = Quaternion.Euler(0, 0, Mathf.Sign(angle)*rotateSpeed) * RotationRoot.rotation;
        }
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
        canvas.enabled = false;
        var kokon = Instantiate(KokonPrefab, transform.position, transform.rotation);
        kokon.unit = this;
        kokon.TargetControl = control;
    }

    public void GenerateNewBody()
    {
        canvas.enabled = true;
        Clear();
        front = Instantiate(Bodytypes.GetRandomFront(), FrontPosition.position, FrontPosition.rotation, FrontPosition);
        front.transform.localPosition = Vector3.zero;
        frontAnims = front.GetComponentsInChildren<Animator>();
        back = Instantiate(Bodytypes.GetRandomBack(), BackPosition.position, BackPosition.rotation, BackPosition);
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
        Dash = front.GetComponentInChildren<Dash_Base>();
        Dash = back.GetComponentInChildren<Dash_Base>();
        
        //init components
        SetInvincible(invincible);
        //Armor.PlayDamageEffect();
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
        foreach (var frontAnim in frontAnims)
        {
            frontAnim.SetBool(anim, on);
        }
        backAnim.SetBool(anim, on);
    }
    
    private void PlayAnim(string anim)
    {
        foreach (var frontAnim in frontAnims)
        {
            frontAnim.SetTrigger(anim);
        }
        backAnim.SetTrigger(anim);
    }

    public bool TryMove(Vector2 direction)
    {
        if (Move is null || !Move.IsCanMove)
        {
            return false;
        }

        //stop movement
        if (direction.sqrMagnitude < 0.5f)
        {
            Move.SetMove(0, 0);
            PlayAnimBool("Move", false);
            return false;
        }
        
        Move.SetMove(direction.x, direction.y);
        PlayAnimBool("Move", true);
        return true;
    }

    public bool TryDash()
    {
        if (Dash is null || !Dash.CanDash)
        {
            return false;
        }
        Dash.Dash();
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
        if (Ability is null || Ability.CanUse() == false)
        {
            return false;
        }
        Ability.Use();
        PlayAnim("Ability");
        return true;
    }

    public void LookAt(Vector2 targetLookPos)
    {
        float angle = Vector2.SignedAngle(RotationRoot.right, targetLookPos - (Vector2)transform.position);
        if (Mathf.Abs(angle) < rotateSpeed)
        {
            return;
        } 
        targetLookAt = targetLookPos - (Vector2)transform.position;
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

    public void SetInvincible(bool set)
    {
        Armor.invincible = set;
    }
    public void Heal(int heal)
    {
        CurrentHP += heal;
        if(CurrentHP > MaxHitPoints)
        {
            CurrentHP = MaxHitPoints;
        }
    }

    public void HealRelative(float healPercent)
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
        GameLoop.Instance.RemoveFromUnitList(this);
        gameObject.SetActive(false);
    }
}
