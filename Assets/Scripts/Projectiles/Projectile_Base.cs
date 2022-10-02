using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Base : MonoBehaviour
{
    public float Speed;
    public float ProjectileLifeTime;
    public float Damage;

    public GameObject ImpactEffect;

    protected Rigidbody2D rb;
    protected CircleCollider2D projectilleCollider;

    protected Unit_Base Parent;

    private void Awake()
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
    
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Unit_Base enemy = collision.transform.root.GetComponent<Unit_Base>();
        if(enemy != null)
        {
            if (enemy == Parent)
            {
                return;
            }
            enemy.TakeDamage(Damage);
        }
        OnProjectileLifeEnd();
    }
    
    public virtual void Spawn(Unit_Base parent, float damage)
    {

        Damage = damage;
        Parent = parent;
        Physics2D.IgnoreCollision(parent.GetComponent<Collider2D>(), projectilleCollider);
        rb.AddForce(transform.right * Speed,ForceMode2D.Force);
    }

    private void OnProjectileLifeEnd()
    {
        if (ImpactEffect != null)
        {
            Instantiate(ImpactEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
