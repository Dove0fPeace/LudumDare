using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Base : MonoBehaviour
{
    public float DashCooldown = 3f;
    public float dashCurrentCooldown;
    public float DashForce;
    public float DashMoveBlock = 1f;
    public bool InvincibleInDash;

    public bool CanDash = true;

    public bool DashNow = false;

    public Rigidbody2D rb;
    public Move_Base Move_Target;
    public Unit_Base self;

    public Player_HUD hud;

    public Timer dashTimer;
    

    protected virtual void Start()
    {
        rb = transform.root.GetComponent<Rigidbody2D>();
        self = transform.root.GetComponent<Unit_Base>();
        Move_Target = self.Move;

        hud = Player_HUD.Instance;
        hud.InitUI(ObjWithCooldown.Dash, null);

        dashTimer = Timer.CreateTimer(DashCooldown, false);
        dashTimer.OnTick += UpdateDashUI;
    }

    private void OnDestroy()
    {
        dashTimer.OnTick -= UpdateDashUI;
        dashTimer.Destroy();
    }

    protected virtual void UpdateDashUI()
    {
        dashCurrentCooldown = dashTimer.CurrentTime / DashCooldown;
        hud.UpdateCooldown(ObjWithCooldown.Dash, dashCurrentCooldown);
    }

    public virtual bool Dash()
    {
        if (!CanDash)
        {
            return false;
        }
        rb.AddForce(DashForce * transform.right, ForceMode2D.Impulse);
        StartCoroutine(Dashing());
        dashTimer.Play();
        return true;
    }

    protected virtual IEnumerator Dashing()
    {
        self.SetInvincible(InvincibleInDash);
        DashNow = true;
        Move_Target.SetLayer(LayerMask.NameToLayer("bugDash"));
        Move_Target.IsCanMove = false;
        CanDash = false;
        yield return new WaitForSeconds(DashMoveBlock);
        rb.Sleep();
        Move_Target.IsCanMove = true;
        Move_Target.SetLayer(LayerMask.NameToLayer("bug"));
        yield return new WaitForSeconds(DashCooldown - DashMoveBlock);
        self.SetInvincible(false);
        DashNow = false;
        CanDash = true;
        hud.InitUI(ObjWithCooldown.Dash, null);
        dashTimer.Pause();
        dashTimer.Restart();

    }
}
