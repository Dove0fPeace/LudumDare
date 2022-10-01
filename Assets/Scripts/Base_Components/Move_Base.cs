using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Base : MonoBehaviour
{
    public Rigidbody2D rb;

    public float MoveSpeed;
    public float DiagonalSpeedLimit = 0.7f;

    public bool IsCanMove;

    float inputHorizontal;
    float inputVertical;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        if(Input.GetAxisRaw("Fire3") != 0)
        {
            Dash();
        }
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
            rb.velocity = new Vector2(inputHorizontal * MoveSpeed, inputVertical * MoveSpeed);
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
