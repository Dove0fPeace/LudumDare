using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Base : MonoBehaviour
{
    public float Speed;
    private int Damage;

    public GameObject ImpactEffect;

    private Rigidbody2D rb;

    private Unit_Base Parent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
        float stepLenght = Time.deltaTime * Speed;
        Vector2 step = transform.up * stepLenght;
        transform.position += new Vector3(step.x, step.y, 0);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Unit_Base enemy = collision.transform.root.GetComponent<Unit_Base>();
        if(enemy != null && enemy != Parent)
        {
            enemy.TakeDamage(Damage);
            OnProjectileLifeEnd();
        }
    }
    public void Spawn(Unit_Base parent, int damage)
    {

        Damage = damage;
        Parent = parent;
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
