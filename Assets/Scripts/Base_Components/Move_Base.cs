using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Move_Base : MonoBehaviour
{
    public BodyPart Part = BodyPart.Legs;

    public Rigidbody2D rb;

    public float MoveSpeed;
    public float DiagonalSpeedLimit = 0.7f;

    public bool IsCanMove = true;
    public bool CanDash = true;

    float inputHorizontal;
    float inputVertical;


    [Header("Dash")]
    public float DashCooldown = 3f;

    public float DashImpulse = 300f;
    public float DashMoveBlock = 1f;


    private List<float> speedCoefs = new List<float>(3);
    public virtual Insects InsectType => Insects.Generic;

    private void Start()
    {
        rb = transform.root.GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if(IsCanMove)
        {
            Move();
        }
    }

    public void SetMove(float horizontal, float vertical)
    {
        inputHorizontal = horizontal;
        inputVertical = vertical;
    }


    public void Move()
    {
        if (inputHorizontal != 0 || inputVertical != 0)
        {
            var direction = GetInputDirection();
            rb.velocity = direction * MoveSpeed * SpeedModifier() * Time.deltaTime;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private Vector2 GetInputDirection()
    {
        if (inputHorizontal != 0 && inputVertical != 0)
        {
            inputHorizontal *= DiagonalSpeedLimit;
            inputVertical *= DiagonalSpeedLimit;
        }

        Vector2 direction = new Vector2(inputHorizontal, inputVertical);
        return direction;
    }

    public float SpeedModifier()
    {
        if (speedCoefs.Count == 0)
        {
            return 1f;
        }
        float speedPenalty = speedCoefs[0];
        float speedBoost = speedCoefs[^1];
        float speedModifier = (speedPenalty + speedBoost) / 2f;
        Debug.LogError(speedModifier);
        return speedModifier;
    }

    public virtual bool Dash()
    {
        if (!CanDash)
        {
            return false;
        }
        rb.AddForce(DashImpulse*GetInputDirection(), ForceMode2D.Impulse);
        StartCoroutine(Dashing());
        return true;
    }

    IEnumerator Dashing()
    {
        rb.gameObject.layer = LayerMask.NameToLayer("bugDash");
        IsCanMove = false;
        CanDash = false;
        yield return new WaitForSeconds(DashMoveBlock);
        IsCanMove = true;
        yield return new WaitForSeconds(DashCooldown - DashMoveBlock);
        rb.gameObject.layer = LayerMask.NameToLayer("bug");
        CanDash = true;
    }

    public void ApplySpeedModifier(float modifier, bool active = true)
    {
        if (active)
        {
            speedCoefs.Add(modifier);
        }
        else
        {
            if (speedCoefs.Contains(modifier))
            {
                speedCoefs.Remove(modifier);
            }
        }
        speedCoefs.Sort();
    }

}
