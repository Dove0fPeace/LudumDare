using Base_Components;
using UnityEngine;
using Random = UnityEngine.Random;

public class Unit_Base : MonoBehaviour
{
    public Bodytypes Bodytypes;

    private HP_Base HP;
    private Move_Base Move;
    private Attack_Base Attack;
    private IAbility Ability;

    [Header("BodyPosition")]
    public Transform HandsPosition;
    public Transform BackPosition;

    private GameObject front;
    private GameObject back;
    
    public Vector2 targetLookPos;

    private void Start()
    {
        ChangeBody();
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
        HP = back.GetComponentInChildren<HP_Base>();
        Move = front.GetComponentInChildren<Move_Base>();
        Attack = front.GetComponentInChildren<Attack_Base>();
        Attack.HandsPlace = HandsPosition;
        Ability = back.GetComponentInChildren<IAbility>();
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
        HP.ChangeHP(damage);
    }
}
