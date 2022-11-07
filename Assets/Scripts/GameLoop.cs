using Base_Components;
using Controls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DefaultNamespace;
using TMPro;
using UnityEngine.Serialization;

public class GameLoop : SingletonBase<GameLoop>
{
    public float mainTime = 10f;
    public AudioClip AudioOnLoopTimesEnd;
    public TMP_Text textBox;
    public Canvas intro, outro;
    public bool GameOn;

    private AudioSource audioSource;

    public PlayerControl PlayerPrefab;
    public Transform PLayerSpawnPoint;

    public CinemachineVirtualCamera targetCamera;

    [FormerlySerializedAs("SpawwnPlayer")] [Header("Eсли на сцене уже есть жук игрока - убери галку")]
    public bool SpawnPlayer = true;
    public bool GodMode;
    private Timer gameLoopTimer;
    [Header ("Restart scene on death")]
    public bool RespawnSceneOnDeath = false;

    private List<Unit_Base> unitList = new List<Unit_Base>();

    public AI EnemyPrefab;
    public int EnemySpawnCount;

    [Header("Enemies Spawn")] 
    public Transform EnemySpawn;
    private Transform[] enemySpawnPoints;
    
    [Header("Traps")]
    public Transform TrapSpawnPoint;
    private Transform[] trapSpawnPoints;

    public int MinTrapsSpawn;
    public int MaxTrapsSpawn;
    public Zone_Base[] Traps;

    private Transform spawnedPlayer;
    private Unit_Base lastEnemy;
    private Control_Base[] enemies;
    private int storyPhase;

    private void Start()
    {
        spawnedPlayer = null;
        lastEnemy = null;
        audioSource = transform.GetComponent<AudioSource>();
        trapSpawnPoints = TrapSpawnPoint.GetComponentsInChildren<Transform>();
        enemySpawnPoints = EnemySpawn.GetComponentsInChildren<Transform>();
        intro.enabled = false;
        outro.enabled = false;
        storyPhase = StoryContainer.storyPhase;
        gameLoopTimer = Timer.CreateTimer(mainTime, false, true);
        gameLoopTimer.OnTimeRunOut += RandomEffect;
        PauseGame();
        StartCoroutine(StartSequence(StoryContainer.GetEnemiesCount(storyPhase), StoryContainer.GetEnemiesHp(storyPhase), StoryContainer.GetEnemiesBrain(storyPhase), StoryContainer.GetStory(storyPhase), storyPhase == 0));
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
        foreach (var unit in unitList)
        {
            unit.ChangeControlState(false);
        }
    }

    public IEnumerator ShowIntro()
    {
        intro.enabled = true;
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.anyKeyDown);
        intro.enabled = false;
    }

    IEnumerator StartSequence(int enemiesNumber, int enemiesHP, float brain, string[] sequence, bool intro = false)
    {
        PauseGame();
        if (intro)
        {
            yield return StartCoroutine(ShowIntro());
        }
        textBox.transform.parent.gameObject.SetActive(true);
        EnemySpawnCount = enemiesNumber;
        if (spawnedPlayer is null)
        {
            spawnedPlayer = Instantiate(PlayerPrefab, PLayerSpawnPoint.position, PLayerSpawnPoint.rotation).transform;
            targetCamera.Follow = spawnedPlayer;
            spawnedPlayer.GetComponent<Unit_Base>().SetGodMode(GodMode);
        }
        else
        {
            spawnedPlayer.GetComponent<Unit_Base>().HealRelative(1f);
        }
        SpawnEnemy(EnemySpawnCount, enemiesHP, brain);
        yield return new WaitForSeconds(0.8f);
        foreach (var unitBase in unitList)
        {
            unitBase.GetComponent<Control_Base>().enabled = false;
        }
        foreach (var text in sequence)
        {
            textBox.text = text;
            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
        textBox.transform.parent.gameObject.SetActive(false);
        foreach (var unitBase in unitList)
        {
            unitBase.GetComponent<Control_Base>().enabled = true;
        }
        PlayGame();
    }

    private void OnDestroy()
    {
        if(gameLoopTimer != null)
            gameLoopTimer.OnTimeRunOut -= RandomEffect;
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
            storyPhase++;
            if (storyPhase < 5)
            {
                StartCoroutine(StartSequence(StoryContainer.GetEnemiesCount(storyPhase), StoryContainer.GetEnemiesHp(storyPhase), StoryContainer.GetEnemiesBrain(storyPhase),StoryContainer.GetStory(storyPhase)));
            }
            else
            {
                StartCoroutine(EndSequence(StoryContainer.GetStory(5)[0]));
            }
        }
    }

    IEnumerator EndSequence(string text)
    {
        PauseGame();
        textBox.transform.parent.gameObject.SetActive(true);
        textBox.text = StoryContainer.GetStory(5)[0];
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.anyKeyDown);
        outro.enabled = true;
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
