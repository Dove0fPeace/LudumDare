using Controls;
using System.Collections;
using UnityEngine;

public class Dash_Base : MonoBehaviour
{
    public float DashCooldown = 3f;
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

    public AI ai;

    protected virtual void Start()
    {
        var root = transform.root;
        rb = root.GetComponent<Rigidbody2D>();
        self = root.GetComponent<Unit_Base>();
        Move_Target = self.Move;

        dashTimer = Timer.CreateTimer(DashCooldown, false, false);

        ai = root.GetComponent<AI>();
        if(ai == null)
        {
            hud = Player_HUD.Instance;
            hud.InitUI(ObjWithCooldown.Dash, true, dashTimer);
        }
    }

    private void OnDestroy()
    {
        Move_Target.SetLayer(LayerMask.NameToLayer("bug"));
        StopAllCoroutines();
        dashTimer.Destroy();
    }

    public virtual bool Dash()
    {
        if (!CanDash && rb != null)
        {
            return false;
        }
        rb.AddForce(DashForce * transform.right, ForceMode2D.Impulse);
        dashTimer.Play();
        StartCoroutine(Dashing());
        return true;
    }

    protected virtual IEnumerator Dashing()
    {
        self.SetInvincible(InvincibleInDash);
        DashNow = true;
        //cache layer name
        Move_Target.SetLayer(LayerMask.NameToLayer("bugDash"));
        Move_Target.IsCanMove = false;
        CanDash = false;
        yield return new WaitForSeconds(DashMoveBlock);
        rb.Sleep();
        Move_Target.IsCanMove = true;
        Move_Target.SetLayer(LayerMask.NameToLayer("bug"));
        yield return new WaitForSeconds(DashCooldown - DashMoveBlock);
        self.SetInvincible(false);
        //looks like DashNow and CanDash are the same flag)
        DashNow = false;
        CanDash = true;
        dashTimer.Restart(true);
    }
}
