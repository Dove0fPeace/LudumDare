using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLegs : Move_Base
{
    public override Insects InsectType => Insects.Spider;

    public override void Dash()
    {
        print("Speder web dash");
    }
}
