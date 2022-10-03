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
        TargetControl.enabled = false;
    }

    public void DestroyKokon()
    {
        unit.GenerateNewBody();
        TargetControl.enabled=true;
        if(KokonFX != null)
        {
            Instantiate(KokonFX,transform.position, transform.rotation);
        }

        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "Stains";
        spriteRenderer.DOFade(0, 5f).OnComplete(DestroyThis);
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
