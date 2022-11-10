using System;
using Base_Components;
using Controls;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DefaultNamespace;
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

    [SerializeField] private EndGameUI _endGameUI;
    [SerializeField] private StoryController _storyController;

    [Header("GameSettings")] [SerializeField]
    private GameModeSettings _gameModeSettings;
    [SerializeField] private bool GodMode;

    [SerializeField] private bool _respawnSceneOnDeath = false;
    public bool RespawnSceneOnDeath => _respawnSceneOnDeath;

    public bool GameOn { get; private set; }
    private bool endlessGame;
    [HideInInspector]public bool EndlessGame => endlessGame;


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
    public Transform SpawnedPlayer;
    private Unit_Base lastEnemy;
    private Control_Base[] enemies;
    private AudioSource audioSource;

    [HideInInspector]public bool GameIsStarted = false;
    public event Action OnCompleteStorySequence;


    private void Start()
    {
        SpawnedPlayer = null;
        lastEnemy = null;
        audioSource = transform.GetComponent<AudioSource>();
        trapSpawnPoints = TrapSpawnPointsContainer.GetComponentsInChildren<Transform>();
        enemySpawnPoints = EnemySpawnPointsContainer.GetComponentsInChildren<Transform>();
        gameLoopTimer = Timer.CreateTimer(mainTime, false, true);
        gameLoopTimer.OnTimeRunOut += RandomEffect;
        Unit_Base.OnPlayerDead += PauseGame;
        PauseGame();
        endlessGame = _gameModeSettings.EndlessGame;
        if (endlessGame)
        {
            StartGame();
            SpawnEnemy(Random.Range(_enemyMinSpawn, _enemyMaxSpawn+1),
                        StoryContainer.GetEnemiesHp(_gameModeSettings.DifficultyLevel),
                        StoryContainer.GetEnemiesBrain(_gameModeSettings.DifficultyLevel));
            PlayGame();
        }
    }

    private void OnDestroy()
    {
        Unit_Base.OnPlayerDead += PauseGame;
        if(gameLoopTimer != null)
            gameLoopTimer.OnTimeRunOut -= RandomEffect;
    }

    public void StartGame()
    {
        SpawnedPlayer = Instantiate(PlayerPrefab, PLayerSpawnPoint.position, PLayerSpawnPoint.rotation).transform;
        targetCamera.Follow = SpawnedPlayer;
        Unit_Base playerUnit = SpawnedPlayer.GetComponent<Unit_Base>();
        playerUnit.SetGodMode(GodMode);
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
            enemy.target = SpawnedPlayer;
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
            if (endlessGame)
            {
                SpawnEnemy(Random.Range(_enemyMinSpawn, _enemyMaxSpawn+1),
                    StoryContainer.GetEnemiesHp(_gameModeSettings.DifficultyLevel),
                    StoryContainer.GetEnemiesBrain(_gameModeSettings.DifficultyLevel));
            }
            else if(unitList.Contains(SpawnedPlayer.GetComponent<Unit_Base>()))
            {
                print("Game loop invoked complete story");
                OnCompleteStorySequence?.Invoke();
            }
        }
    }

    public void KillAll()
    {
        foreach (var unit in unitList)
        {
            Destroy(unit.gameObject);
        }
        unitList.Clear();
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
            SpawnedPlayer.GetComponent<Unit_Base>().ChangeBody();
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
