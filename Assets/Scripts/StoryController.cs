using System.Collections;
using UnityEngine;
using DefaultNamespace;
using TMPro;

public class StoryController : MonoBehaviour
{
    [SerializeField] private GameModeSettings _gameModeSettings;
    
    [SerializeField] private TMP_Text _textBox;
    [SerializeField] private Canvas _intro, _outro;
    private int storyPhase;

    private Unit_Base _player;

    private GameLoop gameLoop;

    private void Start()
    {
        _intro.enabled = false;
        _outro.enabled = false;
        if(_gameModeSettings.EndlessGame)
            enabled = false;
        
        gameLoop = GameLoop.Instance;
        gameLoop.OnCompleteStorySequence += CompleteSequence;
        Unit_Base.OnPlayerDead += RestartSequence;
        StoryContainer.storyPhase = 0;
        StartCoroutine(StartSequence(StoryContainer.GetEnemiesCount(0),
                                                       StoryContainer.GetEnemiesHp(0),
                                                       StoryContainer.GetEnemiesBrain(0),
                                                       StoryContainer.GetStory(0),
                                                       true));
    }

    private void OnDestroy()
    {
        if (!enabled) return;
        gameLoop.OnCompleteStorySequence -= CompleteSequence;
        Unit_Base.OnPlayerDead -= RestartSequence;
    }

    private void CompleteSequence()
    {
        print("Increment story phase");
        storyPhase++;
        StartCoroutine(storyPhase < 5
            ? StartSequence(StoryContainer.GetEnemiesCount(storyPhase), StoryContainer.GetEnemiesHp(storyPhase),
                StoryContainer.GetEnemiesBrain(storyPhase), StoryContainer.GetStory(storyPhase))
            : EndSequence(StoryContainer.GetStory(5)[0]));
    }

    private void RestartSequence()
    {
        print("Restart Sequence");
        gameLoop.KillAll();
        gameLoop.StartGame();
        StartCoroutine(StartSequence(StoryContainer.GetEnemiesCount(storyPhase),
            StoryContainer.GetEnemiesHp(storyPhase),
            StoryContainer.GetEnemiesBrain(storyPhase), StoryContainer.GetStory(storyPhase)));
    }
    
    private IEnumerator StartSequence(int enemiesNumber, int enemiesHP, float brain, string[] sequence, bool intro = false)
    {
        gameLoop.PauseGame();
        if (intro)
        {
            yield return StartCoroutine(ShowIntro());
        }
        _textBox.transform.parent.gameObject.SetActive(true);
        if (gameLoop.GameIsStarted == false)
        {
            gameLoop.StartGame();
        }
        else
        {
            var player = gameLoop.SpawnedPlayer;
            player.GetComponent<Unit_Base>().HealRelative(1f);
        }
        gameLoop.SpawnEnemy(enemiesNumber, enemiesHP, brain);
        //yield return new WaitForSeconds(0.8f);
        gameLoop.PauseGame();
        foreach (var text in sequence)
        {
            _textBox.text = text;
            yield return new WaitForSeconds(0.4f);
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
        _textBox.transform.parent.gameObject.SetActive(false);
        
        gameLoop.PlayGame();
    }
    
    private IEnumerator ShowIntro()
    {
        _intro.enabled = true;
        yield return new WaitForSeconds(0.8f);
        yield return new WaitUntil(() => Input.anyKeyDown);
        _intro.enabled = false;
    }
    
    private IEnumerator EndSequence(string text)
    {
        gameLoop.PauseGame();
        _textBox.transform.parent.gameObject.SetActive(true);
        _textBox.text = text;
        yield return new WaitForSeconds(0.8f);
        yield return new WaitUntil(() => Input.anyKeyDown);
        _outro.enabled = true;
    }
}
