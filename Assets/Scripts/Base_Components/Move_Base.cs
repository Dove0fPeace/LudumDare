using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Base : MonoBehaviour
{
    public BodyPart Part = BodyPart.Legs;

    public Rigidbody2D rb;

    public float MoveSpeed;
    public float DiagonalSpeedLimit = 0.7f;

    public bool IsCanMove = true;

    float inputHorizontal;
    float inputVertical;

    [Header("Dash")]
    public float DashCooldown;
    public float DashSpeed;

    private Timer DashCooldownTimer;

    private void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    public void SetMove(float horizontal, float vertical)
    {
        inputHorizontal = horizontal;
        inputVertical = vertical;
    }

    private void FixedUpdate()
    {
        if(IsCanMove)
        {
            Move();
        }
    }

    public void Move()
    {
        if (inputHorizontal != 0 || inputVertical != 0)
        {
            if (inputHorizontal != 0 && inputVertical != 0)
            {
                inputHorizontal *= DiagonalSpeedLimit;
                inputVertical *= DiagonalSpeedLimit;
            }
            rb.velocity = new Vector2(inputHorizontal, inputVertical) * MoveSpeed * Time.deltaTime;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public virtual void Dash()
    {
        print("Dash");
    }
}
