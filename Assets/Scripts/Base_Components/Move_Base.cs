using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Base : MonoBehaviour
{
    public Rigidbody2D rb;

    public float MoveSpeed;
    
    public bool IsCanMove = true;

    float inputHorizontal;
    float inputVertical;


    private List<float> speedCoefs = new List<float>(3);
    public virtual Insects InsectType => Insects.Generic;
    private Unit_Base self;


    private void Start()
    {
        rb = transform.root.GetComponent<Rigidbody2D>();
        self = transform.root.GetComponent<Unit_Base>();
    }

    private void FixedUpdate()
    {
        if(IsCanMove)
        {
            Move();
        }
    }

    private void OnDestroy()
    {
        rb.velocity = Vector2.zero;
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
            var direction = new Vector2(inputHorizontal, inputVertical).normalized;
            rb.velocity = direction * MoveSpeed * SpeedModifier() * Time.deltaTime;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
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
        return speedModifier;
    }

    //This belongs to Dash base))
    public void SetLayer(int layer)
    {
        rb.gameObject.layer = layer;
        Collider2D[] colliders = rb.gameObject.GetComponentsInChildren<Collider2D>();
        foreach (var collider1 in colliders)
        {
            collider1.gameObject.layer = layer;
        }
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
