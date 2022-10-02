using UnityEngine;

public class Projectile_Base : MonoBehaviour
{
    public float Speed;
    public float ProjectileLifeTime;
    private int Damage;

    public GameObject ImpactEffect;

    private Rigidbody2D rb;
    private CircleCollider2D projectilleCollider;

    private Unit_Base Parent;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        projectilleCollider = transform.GetComponent<CircleCollider2D>();
    }

    protected virtual void Update()
    {
        ProjectileLifeTime -= Time.deltaTime;
        if(ProjectileLifeTime <= 0)
        {
            OnProjectileLifeEnd();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        projectilleCollider.isTrigger = false;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Unit_Base enemy = collision.transform.root.GetComponent<Unit_Base>();
        if(enemy != null)
        {
            if (enemy == Parent)
            {
                return;
            }
            OnEnemyHit(enemy);
        }
        OnProjectileLifeEnd();
    }

    protected virtual void OnEnemyHit(Unit_Base unit)
    {
        unit.TakeDamage(Damage);
    }
    
    public void Spawn(Unit_Base parent, int damage)
    {

        Damage = damage;
        Parent = parent;
        rb.AddForce(transform.right * Speed,ForceMode2D.Force);
    }

    protected void OnProjectileLifeEnd()
    {
        if (ImpactEffect != null)
        {
            Instantiate(ImpactEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
