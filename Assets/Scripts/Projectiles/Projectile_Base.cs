using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        projectilleCollider = transform.GetComponent<CircleCollider2D>();
    }

    private void Update()
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
    
    public void Spawn(Unit_Base parent, int damage)
    {

        Damage = damage;
        Parent = parent;
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
