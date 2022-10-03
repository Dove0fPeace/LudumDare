using Controls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kokon : MonoBehaviour
{
    public GameObject KokonFX;
    public Control_Base TargetControl;
    public Unit_Base unit;

    private void Start()
    {
        TargetControl.enabled = false;
    }


    public void DestroyKokon()
    {
        if(KokonFX != null)
        {
            Instantiate(KokonFX,transform.position, transform.rotation);
        }
        unit.GenerateNewBody();
        TargetControl.enabled=true;
        Destroy(gameObject);
    }
}
