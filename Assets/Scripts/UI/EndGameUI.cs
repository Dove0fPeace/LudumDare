using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class EndGameUI : MonoBehaviour
{
    [SerializeField] private RectTransform _endGameUI;

    [SerializeField] private TMP_Text _killCountText;
    [SerializeField] private TMP_Text _timeCountText;

    private void Start()
    {
        Unit_Base.OnPlayerDead += ShowUI;
    }

    private void OnDestroy()
    {
        Unit_Base.OnPlayerDead -= ShowUI;
    }

    private void ShowUI()
    {
        if(!GameLoop.Instance.EndlessGame) return;
        var time = GameLoop.Instance.TimeOnGame;
        var minutes = Mathf.Floor(time / 60).ToString(CultureInfo.InvariantCulture);
        var seconds = (time % 60).ToString("00");
        
        _endGameUI.gameObject.SetActive(true);
        _killCountText.text = $"Kills - {GameLoop.Instance.KillScore}";
        _timeCountText.text = $"Time - {minutes}:{seconds}";
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
