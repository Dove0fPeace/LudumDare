using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bite : Attack_Base
{
    private bool damagePhase = false;
    private Tween tween;
    public override void Attack()
    {
        damagePhase = true;

    }
}
