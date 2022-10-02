using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public float Damage;
    public float Time;

    public Unit_Base Target;
    private Timer timer;

    private void Start()
    {
        timer = Timer.CreateTimer(Time);
        timer.OnTick += DamageTarget;
        print("poison");
    }

    private void OnDestroy()
    {
        timer.OnTick += DamageTarget;
        timer.Destroy();
    }
    public void DamageTarget()
    {
        if(Target == null)return;
        Target.TakeDamage(Damage);
    }
}
