using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : SingletonBase<GameLoop>
{
    public float mainTime = 10f;

    private Timer gameLoopTimer;

    private List<Unit_Base> unitList; 

    private void Start()
    {
        unitList = new List<Unit_Base>();
        gameLoopTimer = Timer.CreateTimer(mainTime, true);
        gameLoopTimer.OnTimeRunOut += RandomEffect;
    }

    private void OnDestroy()
    {
        gameLoopTimer.OnTimeRunOut -= RandomEffect;
    }
    public void AddToUnitList(Unit_Base unit)
    {
        unitList.Add(unit);
    }


    private void RandomEffect()
    {
        InvokeChangeBody();
    }
    private void InvokeChangeBody()
    {
        unitList[Random.Range(0, unitList.Count)].ChangeBody();
    }
}
