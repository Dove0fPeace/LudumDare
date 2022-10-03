using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Base : MonoBehaviour
{
    public float DashCooldown = 3f;
    private float dashCurrentCooldown;
    public float DashForce;
    public float DashMoveBlock = 1f;
    public bool InvincibleInDash;

    public bool CanDash = true;

    private Rigidbody2D rb;
    private Move_Base Move_Target;
    private Unit_Base self;

    private Player_HUD hud;

    private Timer dashTimer;
    

    private void Start()
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

    private void UpdateDashUI()
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

    IEnumerator Dashing()
    {
        self.SetInvincible(InvincibleInDash);
        Move_Target.SetLayer(LayerMask.NameToLayer("bugDash"));
        Move_Target.IsCanMove = false;
        CanDash = false;
        yield return new WaitForSeconds(DashMoveBlock);
        rb.Sleep();
        Move_Target.IsCanMove = true;
        yield return new WaitForSeconds(DashCooldown - DashMoveBlock);
        Move_Target.SetLayer(LayerMask.NameToLayer("bug"));
        self.SetInvincible(false);
        CanDash = true;
        hud.InitUI(ObjWithCooldown.Dash, null);
        dashTimer.Pause();
        dashTimer.Restart();
    }
}
