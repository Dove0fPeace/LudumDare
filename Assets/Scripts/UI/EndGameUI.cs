using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private RectTransform _endGameUI;

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
        if(GameLoop.Instance.EndlessGame)
            _endGameUI.gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
