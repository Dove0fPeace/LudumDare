using UnityEngine;

public class Poison : MonoBehaviour
{
    public Color HP_Bar_Color;

    public float Damage;
    public float TickInterval;

    public int TotalTicks = 3;

    public Unit_Base Target;
    private Timer timer;

    private int currentTicks;

    private void Start()
    {
        timer = Timer.CreateTimer(TickInterval, true, true);
        timer.OnTimeRunOut += DamageTarget;
        Target.ChangeHPBarColor(HP_Bar_Color);
        currentTicks = 0;
    }

    private void OnDestroy()
    {
        Target.ChangeHPBarColor(Target.DefaultHPColor);
        timer.OnTimeRunOut += DamageTarget;
        timer.Destroy();
    }
    public void DamageTarget()
    {
        if(Target == null)return;
        Target.TakeDamage(Damage, true);
        currentTicks++;
        if (currentTicks >= TotalTicks)
        {
            Destroy(gameObject);
        }
    }
}
