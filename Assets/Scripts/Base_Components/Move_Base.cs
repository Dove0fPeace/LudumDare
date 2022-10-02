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

    float inputHorizontal;
    float inputVertical;

    [Header("Dash")]
    public float DashCooldown;
    public float DashSpeed;

    private float dashTime = 0;
    private bool CanDash => dashTime <= 0;

    private List<float> speedCoefs = new List<float>(3);
    public virtual Insects InsectType => Insects.Generic;

    private void Start()
    {
        rb = transform.root.GetComponent<Rigidbody2D>();
        speedCoefs.Add(1f);
    }
    private void Update()
    {
        if (dashTime > 0)
            dashTime -= Time.deltaTime;
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
            if (inputHorizontal != 0 && inputVertical != 0)
            {
                inputHorizontal *= DiagonalSpeedLimit;
                inputVertical *= DiagonalSpeedLimit;
            }
            rb.velocity = new Vector2(inputHorizontal, inputVertical) * MoveSpeed * SpeedModifier() * Time.deltaTime;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public float SpeedModifier()
    {
        float speedModifier = (speedCoefs[1] + speedCoefs[^1]) / 2;
        Debug.LogError(speedModifier);
        return speedModifier;
    }

    public virtual void Dash()
    {
        if(!CanDash) return;
        dashTime = DashCooldown;
        print("Dash");
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
