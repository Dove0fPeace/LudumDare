using UnityEngine;

public class BugBall : Projectile_Base
{
    [Header("Stage Damage Multiplier")]
    public float Stage1_Damage;
    public float Stage2_Damage;
    public float Stage3_Damage;

    [Header("Stage Large Multiplier")]
    public float Stage1_Large;
    public float Stage2_Large;
    public float Stage3_Large;

    [Header("Distance for the End of Stage")]
    public float Stage1_EndDistance;
    public float Stage2_EndDistance;

    private float deltaDistance;
    private Vector2 lastPosition;

    private int ballStage = 1;

    private bool IsLaunched;

    private void Start()
    {

    }
    protected override void Update()
    {
        if(IsLaunched)
        {
            base.Update();
        }
        deltaDistance += Vector2.Distance(Parent.transform.position, lastPosition);
        lastPosition = Parent.transform.position;
        if (IsLaunched == false)
        {
            if (deltaDistance >= Stage1_EndDistance && ballStage == 1)
            {
                ballStage = 2;
                ChangeStage(ballStage);
            }

            if (deltaDistance >= Stage2_EndDistance && ballStage == 2)
            {
                ballStage = 3;
                ChangeStage(ballStage);
            }
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsLaunched) return;
        base.OnCollisionEnter2D(collision);
    }
    public override void Spawn(Unit_Base parent, float damage)
    {
        Damage = damage;
        Parent = parent;
    }

    public void Launch()
    {
        IsLaunched = true;
        rb.AddForce(transform.forward * Speed, ForceMode2D.Force);
        transform.SetParent(null);
    }
    public void ChangeStage(int changeTo)
    {
        switch(changeTo)
        {
            case 1:
                Damage *=  Stage1_Damage;
                transform.localScale = new Vector3(Stage1_Large, Stage1_Large);
                break;
            case 2:
                Damage *= Stage2_Damage;
                transform.localScale = new Vector3(Stage2_Large, Stage2_Large);
                break;
            case 3:
                Damage *= Stage3_Damage;
                transform.localScale = new Vector3(Stage3_Large, Stage3_Large);
                break;
        }
    }
}
