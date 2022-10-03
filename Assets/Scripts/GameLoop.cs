using Base_Components;
using Controls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameLoop : SingletonBase<GameLoop>
{
    public float mainTime = 10f;

    public PlayerControl PlayerPrefab;
    public Transform PLayerSpawnPoint;

    public CinemachineVirtualCamera targetCamera;

    [Header("Eсли на сцене уже есть жук игрока - убери галку")]
    public bool SpawwnPlayer = true;

    private Timer gameLoopTimer;

    private List<Unit_Base> unitList = new List<Unit_Base>();

    public AI EnemyPrefab;
    public int EnemySpawnCount;

    [Header("Traps")]
    public Transform TrapSpawnPoint;
    private Transform[] trapSpawnPoints;

    public int MinTrapsSpawn;
    public int MaxTrapsSpawn;
    public Zone_Base[] Traps;

    private Transform spawnedPlayer;


    private void Start()
    {
        gameLoopTimer = Timer.CreateTimer(mainTime, true);
        gameLoopTimer.OnTimeRunOut += RandomEffect;
        trapSpawnPoints = TrapSpawnPoint.GetComponentsInChildren<Transform>();
        if(SpawwnPlayer)
        {
            spawnedPlayer = Instantiate(PlayerPrefab, PLayerSpawnPoint.position, PLayerSpawnPoint.rotation).transform;
            targetCamera.Follow = spawnedPlayer;
        }
        SpawnEnemy(EnemySpawnCount);
    }

    private void OnDestroy()
    {
        gameLoopTimer.OnTimeRunOut -= RandomEffect;
    }
    public void AddToUnitList(Unit_Base unit)
    {
        unitList.Add(unit);
    }

    public void SpawnEnemy(int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            int point = Random.Range(0, trapSpawnPoints.Length);
            var enemy =  Instantiate(EnemyPrefab, trapSpawnPoints[point].position, trapSpawnPoints[point].rotation);
            enemy.target = spawnedPlayer;
        }
    }

    public void ChangePlayer()
    {
        spawnedPlayer.GetComponent<Unit_Base>().ChangeBody();
    }

    private void RandomEffect()
    {
        int Effect = Random.Range(1, 3);

        switch (Effect)
        {
            case 1:
                InvokeSpawnTrap();
                break;
            case 2:
                InvokeChangeSingleBody();
                break;
            case 3:
                InvokeChangeAllBodies();
                break;
        }
    }

    private void InvokeSpawnTrap()
    {
        int numSpawn = Random.Range(MinTrapsSpawn, MaxTrapsSpawn);
        for (int i = 0; i < numSpawn; i++)
        {
            Instantiate(Traps[Random.Range(0, Traps.Length)], trapSpawnPoints[Random.Range(0, trapSpawnPoints.Length)].position, Quaternion.identity);
        }
    }

    private void InvokeChangeSingleBody()
    {
        unitList[Random.Range(0, unitList.Count)].ChangeBody();
    }

    private void InvokeChangeAllBodies()
    {
        foreach (var unit in unitList)
        {
            unit.ChangeBody();
        }
    }
}
