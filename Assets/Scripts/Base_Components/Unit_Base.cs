using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Base : MonoBehaviour
{
    public Bodytypes Bodytypes;

    [Header("Default BodyParts")]
    public HP_Base HP;
    public Move_Base Move;
    public Attack_Base Attack;
    public Ability_Base Ability;

    [Header("BodyPosition")]
    public Transform HandsPosition;
    public Transform LegsPosition;
    public Transform BackPosition;
    public Transform AbilityPosition;

    public Vector2 targetLookPos;

    private void Update()
    {
        LookAtTarget();
    }
    public void ChangeBody()
    {
        Destroy(HP.gameObject);
        HP = Instantiate(Bodytypes.Backs[Random.Range(0, Bodytypes.Backs.Length)], BackPosition.position, BackPosition.rotation, transform);

        Destroy(Move.gameObject);
        Move = Instantiate(Bodytypes.Legs[Random.Range(0, Bodytypes.Legs.Length)], LegsPosition.position, LegsPosition.rotation, transform);

        Destroy(Attack.gameObject);
        Attack = Instantiate(Bodytypes.Hands[Random.Range(0, Bodytypes.Hands.Length)], HandsPosition.position, HandsPosition.rotation, transform);

        Destroy(Ability.gameObject);
        Ability = Instantiate(Bodytypes.Abilities[Random.Range(0, Bodytypes.Abilities.Length)], AbilityPosition.position, AbilityPosition.rotation, transform);
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
        if (Attack is null)
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
        Ability.UseAbility();
        return true;
    }

    public void LookAtTarget()
    {
        targetLookPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = targetLookPos - new Vector2 (transform.position.x, transform.position.y);
    }
}
