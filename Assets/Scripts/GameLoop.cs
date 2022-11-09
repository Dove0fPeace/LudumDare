using System;
using Base_Components;
using Controls;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using Random = UnityEngine.Random;

public class GameLoop : SingletonBase<GameLoop>
{
    [SerializeField] private float mainTime = 10f;
    [SerializeField] private AudioClip AudioOnLoopTimesEnd;
    
    [Header("Cash GameObjects")]
    [SerializeField] private CinemachineVirtualCamera targetCamera;

    [SerializeField] private TMP_Text textBox;

    [SerializeField] private PlayerControl PlayerPrefab;
    [SerializeField] private Transform PLayerSpawnPoint;

    [SerializeField] private AI EnemyPrefab;

    [Header("GameSettings")]
    [SerializeField] private bool GodMode;

    [SerializeField] private bool _respawnSceneOnDeath = false;
    public bool RespawnSceneOnDeath => _respawnSceneOnDeath;

    public bool GameOn { get; private set; }
    private bool endlessGame;


    [Header("Enemies Spawn")]
    [SerializeField] private int _enemyMinSpawn; //for endless mode
    [SerializeField] private int _enemyMaxSpawn; //for endless mode
    [SerializeField] private Transform EnemySpawnPointsContainer;
    private Transform[] enemySpawnPoints;

    [Header("Traps")]
    [SerializeField] private int MinTrapsSpawn ;
    [SerializeField] private int MaxTrapsSpawn;

    [SerializeField] private Transform TrapSpawnPointsContainer;
    [SerializeField] private Zone_Base[] Traps;
    private Transform[] trapSpawnPoints;


    private Timer gameLoopTimer;
    private List<Unit_Base> unitList = new List<Unit_Base>();
    private Transform spawnedPlayer;
    public Transform SpawnedPlayer => spawnedPlayer;
    private Unit_Base lastEnemy;
    private Control_Base[] enemies;
    private AudioSource audioSource;

    [HideInInspector]public bool GameIsStarted = false;
    public event Action OnCompleteStorySequence;


    private void Start()
    {
        spawnedPlayer = null;
        lastEnemy = null;
        audioSource = transform.GetComponent<AudioSource>();
        trapSpawnPoints = TrapSpawnPointsContainer.GetComponentsInChildren<Transform>();
        enemySpawnPoints = EnemySpawnPointsContainer.GetComponentsInChildren<Transform>();
        gameLoopTimer = Timer.CreateTimer(mainTime, false, true);
        gameLoopTimer.OnTimeRunOut += RandomEffect;
        PauseGame();
        if (endlessGame)
        {
            StartGame();
        }
    }

    private void OnDestroy()
    {
        if(gameLoopTimer != null)
            gameLoopTimer.OnTimeRunOut -= RandomEffect;
    }

    public void StartGame()
    {
        spawnedPlayer = Instantiate(PlayerPrefab, PLayerSpawnPoint.position, PLayerSpawnPoint.rotation).transform;
        targetCamera.Follow = spawnedPlayer;
        spawnedPlayer.GetComponent<Unit_Base>().SetGodMode(GodMode);
        GameIsStarted = true;
    }

    public void PlayGame()
    {
        GameOn = true;
        gameLoopTimer.Play();
        foreach (var unit in unitList)
        {
            unit.ChangeControlState(true);
        }
    }

    public void PauseGame()
    {
        GameOn = false;
        gameLoopTimer.Restart(true);
        if (unitList.Count == 0) return;
        foreach (var unit in unitList)
        {
            unit.ChangeControlState(false);
        }
    }

    public void SpawnEnemy(int spawnCount, int enemiesHp, float brain)
    {
        enemies = new Control_Base[spawnCount];
        for (int i = 0; i < spawnCount; i++)
        {
            int point = Random.Range(0, enemySpawnPoints.Length);
            var enemy =  Instantiate(EnemyPrefab, enemySpawnPoints[point].position, enemySpawnPoints[point].rotation);
            enemy.target = spawnedPlayer;
            enemy.smartness = brain;
            enemies[i] = enemy;
            lastEnemy = enemy.GetComponent<Unit_Base>();
            lastEnemy.MaxHitPoints = enemiesHp;
        }
    }

    public void AddToUnitList(Unit_Base unit)
    {
        unitList.Add(unit);
    }

    public void RemoveFromUnitList(Unit_Base unit)
    {
        unitList.Remove(unit);
        if (unitList.Count <= 1)
        {
            OnCompleteStorySequence?.Invoke();
        }
    }

    #region FromUiDebug

        public void ChangeEnemy()
        {
            if (lastEnemy is null)
            {
                SpawnEnemy(1, 10, 3);
            }
            lastEnemy.ChangeBody();
        }

        public void ChangePlayer()
        {
            spawnedPlayer.GetComponent<Unit_Base>().ChangeBody();
        }

    #endregion

    private void RandomEffect()
    {
        audioSource.PlayOneShot(AudioOnLoopTimesEnd);

        InvokeChangeAllBodies();
        
        int Effect = Random.Range(1, 3);
        switch (Effect)
        {
            case 1:
                InvokeSpawnTrap();
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

    private void InvokeChangeAllBodies()
    {
        foreach (var unit in unitList)
        {
            unit.ChangeBody();
        }
    }

    private void InvokeChangeSingleBody()
    {
        unitList[Random.Range(0, unitList.Count)].ChangeBody();
    }
}
