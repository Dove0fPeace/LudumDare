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
    public bool CanDash = true;

    float inputHorizontal;
    float inputVertical;


    [Header("Dash")]
    
    public float DashCooldown = 3f;
    private float dashCurrentCooldown;
    public float DashForce;
    public float DashMoveBlock = 1f;
    public bool InvincibleInDash;

    private Player_HUD hud;
    private Timer dashTimer;


    private List<float> speedCoefs = new List<float>(3);
    public virtual Insects InsectType => Insects.Generic;
    private Unit_Base self;


    private void Start()
    {
        rb = transform.root.GetComponent<Rigidbody2D>();
        self = transform.root.GetComponent<Unit_Base>();
        hud = Player_HUD.Instance;
        hud.InitUI(ObjWithCooldown.Dash, null);
        dashTimer = Timer.CreateTimer(DashCooldown,false);
        dashTimer.OnTick += UpdateDashUI;
    }

    private void UpdateDashUI()
    {
        dashCurrentCooldown = dashTimer.CurrentTime;
        hud.UpdateCooldown(ObjWithCooldown.Dash, (int)dashCurrentCooldown);
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
        dashTimer.OnTick -= UpdateDashUI;
        dashTimer.Destroy();
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
        return speedModifier;
    }

    public virtual bool Dash()
    {
        if (!CanDash)
        {
            return false;
        }
        rb.AddForce(DashForce*transform.right, ForceMode2D.Impulse);
        StartCoroutine(Dashing());
        dashTimer.Play();
        return true;
    }

    IEnumerator Dashing()
    {
        self.Armor.invincible = InvincibleInDash;
        SetLayer(LayerMask.NameToLayer("bugDash"));
        IsCanMove = false;
        CanDash = false;
        yield return new WaitForSeconds(DashMoveBlock);
        rb.Sleep();
        IsCanMove = true;
        yield return new WaitForSeconds(DashCooldown - DashMoveBlock);
        SetLayer(LayerMask.NameToLayer("bug"));
        self.Armor.invincible = false;
        CanDash = true;
        hud.InitUI(ObjWithCooldown.Dash, null);
        dashTimer.Pause();
        dashTimer.Restart();
    }

    private void SetLayer(int layer)
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
