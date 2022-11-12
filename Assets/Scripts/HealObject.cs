using Controls;
using UnityEngine;
using Random = UnityEngine.Random;

public class HealObject : MonoBehaviour
{
    [Header("Heal power 1-3")]
    [SerializeField] private int _healPower;

    private void Start()
    {
        _healPower = Random.Range(1, 4);
        transform.localScale *= 1f +_healPower/8;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.transform.root.GetComponent<PlayerControl>();
        if (player == null) return;

        Heal(player.CurrentUnit);
        Destroy(gameObject);
    }

    private void Heal(Unit_Base target)
    {
        switch (_healPower)
        {
            case 1:
                target.HealRelative(0.15f);
                break;
            case 2:
                target.HealRelative(0.2f);
                break;
            case 3:
                target.HealRelative(0.3f);
                break;
            default:
                target.HealRelative(0.1f);
                break;
        }
    }
}
