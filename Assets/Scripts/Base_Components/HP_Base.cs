using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HP_Base : MonoBehaviour
{
    public BodyPart Part = BodyPart.Back;

    [SerializeField] private int _MaxHitPoints;
    private int _currentHitPoint;

    private void Start()
    {
        _currentHitPoint = _MaxHitPoints;
    }

    public void ChangeHP(int damage)
    {
        _currentHitPoint -= damage;
        print(_currentHitPoint + "/" + _MaxHitPoints);
        if( _currentHitPoint <= 0 )
        {
            Death();
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
