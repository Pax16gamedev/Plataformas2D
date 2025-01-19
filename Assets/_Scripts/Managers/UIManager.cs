using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Top Container")]
    [SerializeField] private TextMeshProUGUI timeTMP;
    [SerializeField] private TextMeshProUGUI scoreTMP;

    [Header("Paneles")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject levelEndScreenPanel;
    [SerializeField] private GameObject deathScreenPanel;

    [Header("Delay")]
    [SerializeField] float deathScreenDelay = 1;

    [Header("Info paneles")]
    [SerializeField] EndScreenInfo endScreenInfo;

    [Header("Bar")]
    [SerializeField] HealthBar healthBar;
    [SerializeField] DashBar dashBar;

    public HealthBar HealthBar { get => healthBar; }
    public DashBar DashBar { get => dashBar; }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UpdateScoreText(0);
        GameManager.Instance.CheckForSceneGameObjects();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            ShowPausePanel();
        }
    }

    public void UpdateTimeText(float text)
    {
        timeTMP.text = $"tiempo: {text:F2} s";
    }

    public void UpdateScoreText(int score)
    {
        scoreTMP.text = $"Puntos: {score.ToString()}";
    }

    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
        GameManager.Instance.PauseGame();
        Time.timeScale = 0;
    }

    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
        GameManager.Instance.ResumeGame();
    }

    public void GoToMainMenu()
    {
        GameManager.Instance.ResumeGame();
        GameManager.Instance.ResetVariables();
        SceneManager.LoadScene(0);
    }

    public void ShowLevelEndScreen(int stars, float timeTaken, float bestTime)
    {
        endScreenInfo.ShowInfo(stars, timeTaken, bestTime);
        levelEndScreenPanel.SetActive(true);
    }

    public void HideLevelEndScreen() => levelEndScreenPanel.SetActive(false);

    public void ShowDeathScreen()
    {
        StartCoroutine(DeathScreen());
    }

    private IEnumerator DeathScreen()
    {
        yield return new WaitForSeconds(deathScreenDelay);
        deathScreenPanel.SetActive(true);
        GameManager.Instance.PauseGame();
    }

    public void HideDeathScreen() => deathScreenPanel.SetActive(false);

    public void GoToNextLevel()
    {
        GameManager.Instance.GoToNextLevel();
    }

    public void RetryLevel()
    {
        GameManager.Instance.RestartLevel();
        HideDeathScreen();
    }

}
