using System;
using Controls;
using DG.Tweening;
using UnityEngine;

public class Kokon : MonoBehaviour
{
    public GameObject KokonFX;
    public Control_Base TargetControl;
    public Unit_Base unit;

    private void Start()
    {
        unit.ChangeControlState(false);
    }

    public void DestroyKokon()
    {
        unit.GenerateNewBody();
        if(KokonFX != null)
        {
            Instantiate(KokonFX,transform.position, transform.rotation);
        }
        
        unit.ChangeControlState(true);

        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "Stains";
        spriteRenderer.DOFade(0, 5f).OnComplete(DestroyThis);
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
