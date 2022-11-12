using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameOverlayUI : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] private RectTransform _pauseUI;
    [SerializeField] private TMP_Text _pauseTimeCountText;
    
    [Header("End game")]
    [SerializeField] private RectTransform _endGameUI;
    [SerializeField] private TMP_Text _endGameKillCountText;
    [SerializeField] private TMP_Text _endGameTimeCountText;

    private bool isPaused;

    private void Start()
    {
        isPaused = false;
        Unit_Base.OnPlayerDead += ShowEndGame;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangePauseState();
        }
    }

    private void OnDestroy()
    {
        Unit_Base.OnPlayerDead -= ShowEndGame;
    }

    private void ShowEndGame()
    {
        if(!GameLoop.Instance.EndlessGame) return;
        var time = GameLoop.Instance.TimeOnGame;
        var minutes = Mathf.Floor(time / 60).ToString(CultureInfo.InvariantCulture);
        var seconds = (time % 60).ToString("00");
        
        _endGameUI.gameObject.SetActive(true);
        _endGameKillCountText.text = $"Kills - {GameLoop.Instance.KillScore}";
        _endGameTimeCountText.text = $"Time - {minutes}:{seconds}";
    }

    public void ChangePauseState()
    {
        switch (isPaused)
        {
            case true:
                isPaused = false;
                GameLoop.Instance.PlayGame();
                _pauseUI.gameObject.SetActive(false);
                break;
            case false:
                isPaused = true;
                var time = GameLoop.Instance.TimeOnGame;
                var minutes = Mathf.Floor(time / 60).ToString(CultureInfo.InvariantCulture);
                var seconds = (time % 60).ToString("00");
        
                GameLoop.Instance.PauseGame();
                _pauseUI.gameObject.SetActive(true);
                _pauseTimeCountText.text = $"Time - {minutes}:{seconds}";
                break;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
