using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameModeSettings _settings;

    [SerializeField] private RectTransform _mainMenu;
    [SerializeField] private RectTransform _choseDifficulty;

    public void ChangeMenuVisible()
    {
        _mainMenu.gameObject.SetActive(!_mainMenu.gameObject.activeSelf);
        _choseDifficulty.gameObject.SetActive(!_choseDifficulty.gameObject.activeSelf);
    }

    public void StartStory()
    {
        _settings.EndlessGame = false;
        SceneManager.LoadScene(1);
    }

    public void StartEndless(int difficulty)
    {
        _settings.EndlessGame = true;
        _settings.DifficultyLevel = difficulty;
        SceneManager.LoadScene(1);
    }
}
