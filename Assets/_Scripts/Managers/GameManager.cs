using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private float timeElapsed = 0f;
    private int score = 0;

    private bool levelFinished = false;
    public bool LevelFinished => levelFinished;

    [SerializeField] private LevelInfoSO currentLevelInfo; // Serializado solo para modo editor

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
        UIManager.Instance.UpdateTimeText(timeElapsed);
        UIManager.Instance.UpdateScoreText(score);

        print($"Current level info {currentLevelInfo.ID}");
    }

    private void Update()
    {
        if(levelFinished) return;

        timeElapsed += Time.deltaTime;
        UIManager.Instance.UpdateTimeText(timeElapsed);
    }

    public void LoadLevelInfo(LevelInfoSO levelInfo)
    {
        if(levelInfo == null) return;

        currentLevelInfo = levelInfo;
    }

    public void FinishLevel()
    {
        levelFinished = true;

        int estrellas = currentLevelInfo.CalculateStars(timeElapsed);
        Debug.Log("Nivel completado con " + estrellas + " estrellas.");

        //UIManager.Instance.ShowLevelEndScreen(estrellas, score);
    }
}
