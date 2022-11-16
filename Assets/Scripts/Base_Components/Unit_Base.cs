using System;
using System.Collections;
using System.Collections.Generic;
using Base_Components;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Base : MonoBehaviour
{
    
    //The hp bar is UI, make a separate script for it and let it connect to the Unit_Base monobeh and subscribe to its HPchanged event.
    [Header("HP")]
    public int MaxHitPoints;
    public float CurrentHP;
    public Canvas UnitHpView;
    public Slider HpBar;
    private RectTransform fillRect;
    public Color DefaultHPColor;

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

    private List<SpriteRenderer> _sprites;

    private AudioSource audioSource;
    private Vector3 initialPosition;
    private bool invincible;

    public Kokon KokonPrefab;
    private bool isControl;

    private Vector2 targetLookAt;
    private float rotateSpeed = 5f;

    private Player_HUD PlayerHUD;

    //don't use static events
    public static event Action OnPlayerDead;


    private void Start()
    {
        if (transform.root.CompareTag("Player"))
            PlayerHUD = Player_HUD.Instance;
        ChangeBody();
        CurrentHP = MaxHitPoints;
        HpBar.maxValue = MaxHitPoints;
        HpBar.value = HpBar.maxValue;
        initialPosition = transform.position;
        audioSource = GetComponent<AudioSource>();

        fillRect = HpBar.fillRect;

        GameLoop.Instance.AddToUnitList(this);

    }

    private void OnDestroy()
    {
        audioSource.Stop();
    }

    public void SetGodMode(bool on = true)
    {
        invincible = on;
    }

    private void Update()
    {
        HpBar.value = CurrentHP;

        //It would be really nice to have all the rotation functionality in a separate script placed on RotationRoot,
        //we can set its targetLookAt value through this script (right now it's done in the LookAt()).
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
            //respawn - make a separate method
            transform.position = initialPosition;
            CurrentHP = MaxHitPoints;
            HpBar.maxValue = MaxHitPoints;
            HpBar.value = HpBar.maxValue;
            gameObject.SetActive(true);
        }

        Clear();
        UnitHpView.enabled = false;
        var kokon = Instantiate(KokonPrefab, transform.position, transform.rotation);
        kokon.unit = this;
    }

    public void GenerateNewBody()
    {
        
        UnitHpView.enabled = true;
        Clear();
        //Okey, we have lots of functionality here and it's basically the same for front and back
        //We can convert front and back into one separate script (BodyPart) and place it on the back and front root objects.
        //It will have a reference to BodyTypes and have methods like 'Create()', 'Clear()', 'PlayAnimation()'
        front = Instantiate(Bodytypes.GetRandomFront(), FrontPosition.position, FrontPosition.rotation, FrontPosition);
        front.transform.localPosition = Vector3.zero;
        _sprites.Add(front.GetComponent<SpriteRenderer>());
        frontAnims = front.GetComponentsInChildren<Animator>();
        back = Instantiate(Bodytypes.GetRandomBack(), BackPosition.position, BackPosition.rotation, BackPosition);
        back.transform.localPosition = Vector3.zero;
        _sprites.Add(back.GetComponent<SpriteRenderer>());
        backAnim = back.GetComponent<Animator>();
        
        //This is an interesting case, we can probably have one method like Armor = Select<Armor_Base>(front, back);
        //(priorities: move and attack from front, armor and ability from back)
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
        
        SetInvincible(invincible);
    }

    public void ChangeControlState(bool state)
    {
        isControl = state;
        if (isControl == false && Move != null)
        {
            Move.SetMove(0,0);
            Move.rb.velocity = Vector2.zero;
            PlayAnimBool("Move", false);
        }
    }
    public void PlayAudioOneshot(AudioClip clip)
    {
        if(audioSource != null)
            audioSource.PlayOneShot(clip);
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

        if (_sprites == null)
        {
            _sprites = new List<SpriteRenderer>();
        }
        
        _sprites.Clear();
        
        Armor = null;
        Move = null;
        Attack = null;
        Ability = null;
        Dash = null;
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
        if (isControl == false)
        {
            return false;
        }
        if (Move is null || !Move.IsCanMove)
        {
            return false;
        }

        if (direction.sqrMagnitude < 0.5f)
        {
            Move.SetMove(0, 0);
            PlayAnimBool("Move", false);
            return false;
        }
        
        //cache animator parameters: moveAnimationKey = Animator.StringToHash("Move")
        //company guys love that)
        Move.SetMove(direction.x, direction.y);
        PlayAnimBool("Move", true);
        return true;
    }

    public bool TryDash()
    {
        if (isControl == false)
        {
            return false;
        }
        if (Dash is null || !Dash.CanDash)
        {
            if(PlayerHUD != null)
                PlayerHUD.TryUseOnCooldown(ObjWithCooldown.Dash);
            return false;
        }
        Dash.Dash();
        PlayAnimBool("Move", true);
        return true;
    }

    public bool TryAttack()
    {
        if (isControl == false)
        {
            return false;
        }
        if (Attack is null || Attack.Attack() == false)
        {
            if(PlayerHUD != null)
                PlayerHUD.TryUseOnCooldown(ObjWithCooldown.Attack);
            return false;
        }
        Attack.Attack();
        PlayAnim("Attack");
        return true;
    }

    public bool TryAbility()
    {
        if (isControl == false)
        {
            return false;
        }
        if (Ability is null || Ability.CanUse() == false)
        {
            if(PlayerHUD != null)
                PlayerHUD.TryUseOnCooldown(ObjWithCooldown.Ability);
            return false;
        }
        Ability.Use();
        PlayAnim("Ability");
        return true;
    }

    public void LookAt(Vector2 targetLookPos)
    {
        if(isControl == false) return;
        float angle = Vector2.SignedAngle(RotationRoot.right, targetLookPos - (Vector2)transform.position);
        if (Mathf.Abs(angle) < rotateSpeed)
        {
            return;
        } 
        targetLookAt = targetLookPos - (Vector2)transform.position;
    }
    
    //There is lots of functionality related to damage taking, we could try moving it into a separate script.
    public void TakeDamage(float damage, bool isPoison)
    {
        if (isControl == false)
        {
            return;
        }
        if(!isPoison)
        {
            CurrentHP -= Armor.CalculateDamage(damage);
            StartCoroutine(TakeDamageView());
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
    
    //Oh no, we should have a ChangeStatus event as well for HPBar to color itself)
    public void ChangeHPBarColor(Color color)
    {
        fillRect.GetComponent<Image>().color = color;
    }

    private void Death()
    {
        if (PlayerHUD != null)
        {
            OnPlayerDead?.Invoke();
        }
        //gameObject.SetActive(false);
        GameLoop.Instance.RemoveFromUnitList(this);
        Destroy(gameObject);
    }

    //same event for ChangeHP would work here, back and front scripts can listen to it to change sprite color.
    private IEnumerator TakeDamageView()
    {
        foreach (var sprite in _sprites)
        {
            sprite.color = Color.red;
        }
        
        //magic number
        yield return new WaitForSeconds(0.15f);
        
        foreach (var sprite in _sprites)
        {
            sprite.color = Color.white;
        }
    }
}
