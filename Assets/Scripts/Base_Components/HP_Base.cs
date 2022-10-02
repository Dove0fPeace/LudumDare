using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HP_Base : MonoBehaviour
{
    public BodyPart Part = BodyPart.Back;

    [SerializeField] private int _MaxHitPoints;
    [SerializeField] private GameObject vfx;
    private int _currentHitPoint;

    private void Start()
    {
        _currentHitPoint = _MaxHitPoints;
    }

    public void ChangeHP(int damage)
    {
        _currentHitPoint -= damage;
        print(_currentHitPoint + "/" + _MaxHitPoints);
        Instantiate(vfx, transform.root.position, Quaternion.Euler(0, 90, 0));
        if( _currentHitPoint <= 0 )
        {
            Death();
        }
    }

    public void Death()
    {
        Destroy(transform.root.gameObject);
    }
}
