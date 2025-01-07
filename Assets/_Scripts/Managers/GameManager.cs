using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private float timeElapsed = 0f;
    private int score = 0;

    private bool levelStarted = false;
    public bool LevelStarted => levelStarted;

    private bool levelFinished = false;
    public bool LevelFinished => levelFinished;

    [SerializeField] private LevelInfoSO currentLevelInfo; // Serializado solo para modo editor
    public event Action OnLevelChanged;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CanStartLevel();

        if(!levelStarted) return;

        UIManager.Instance.UpdateTimeText(timeElapsed);
        UIManager.Instance.UpdateScoreText(score);

        print($"Current level info {currentLevelInfo.ID}");
    }

    private void Update()
    {
        if(levelFinished) return;
        if(!levelStarted) return;
        if(!UIManager.Instance) return;

        timeElapsed += Time.deltaTime;
        UIManager.Instance.UpdateTimeText(timeElapsed);
    }

    public void LoadLevelInfo(LevelInfoSO levelInfo)
    {
        if(levelInfo == null) return;

        currentLevelInfo = levelInfo;
        OnLevelChanged?.Invoke();
    }

    private void CanStartLevel()
    {
        if(currentLevelInfo == null || currentLevelInfo.ID == 0) // Importante: ID 0 -> Menu principal
        {
            levelStarted = false;
            return;
        }

        levelStarted = true;
    }

    public void FinishLevel()
    {
        levelFinished = true;

        int estrellas = currentLevelInfo.CalculateStars(timeElapsed);
        Debug.Log("Nivel completado con " + estrellas + " estrellas.");

        //UIManager.Instance.ShowLevelEndScreen(estrellas, score);
    }

    private void OnEnable()
    {
        OnLevelChanged += CheckCurrentLevelInfo;
    }

    private void OnDisable()
    {
        OnLevelChanged -= CheckCurrentLevelInfo;
    }

    private void CheckCurrentLevelInfo()
    {
        CanStartLevel();
    }
}
