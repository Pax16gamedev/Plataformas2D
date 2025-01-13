using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Data")]
    [SerializeField] private LevelInfoSO currentLevelInfo; // Serializado solo para modo editor
    private GameData gameData;
    private LevelData levelData;

    [Header("Level State")]
    private float timeElapsed = 0f;
    private int score = 0;
    private bool levelStarted = false;
    private bool levelFinished = false;

    public GameData GameData => gameData;
    public bool LevelStarted => levelStarted;
    public bool LevelFinished => levelFinished;
    public int TotalLevels => SceneManager.sceneCountInBuildSettings - 1; // Excluyo el menu principal.


    public event Action OnLevelChanged; // El nivel ha cambiado
    public event Action OnLevelProgressChanged; // El progreso del nivel ha cambiado


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
        LoadGame();
        LoadLevelDataIfNecessary();

        CanStartLevel();

        if(!levelStarted) return;

        UIManager.Instance.UpdateTimeText(timeElapsed);
        UIManager.Instance.UpdateScoreText(score);

        print($"Current level info {currentLevelInfo.ID}");
    }

    private void Update()
    {
        if(levelFinished || !levelStarted || !UIManager.Instance) return;

        timeElapsed += Time.deltaTime;
        UIManager.Instance.UpdateTimeText(timeElapsed);
    }

    #region ---- Save/Load ----
    public void SaveGame()
    {
        SaveSystem.SaveGame(gameData);
    }

    public void LoadGame()
    {
        gameData = SaveSystem.LoadGame();
        OnLevelProgressChanged?.Invoke();
    }
    #endregion

    #region ---- Level Management ----
    private void LoadLevelDataIfNecessary()
    {
        // Verificar si es la primera vez que se juega y si los datos de los niveles necesitan inicializacion.
        if(IsFirstGameplay())
        {
            InitializeLevels(TotalLevels);
        }
    }
    private bool IsFirstGameplay()
    {
        return gameData == null || gameData.levels == null || gameData.levels.Count == 0;
    }

    private void InitializeLevels(int totalLevels)
    {
        gameData = new GameData();
        for(int i = 1; i <= totalLevels; i++)
        {
            if(GameData.levels.Exists(l => l.levelNumber == i)) continue; // Evitar duplicados.

            LevelData levelData = new LevelData
            {
                levelNumber = i,
                timeTaken = 0f,
                monstersKilled = 0,
                score = 0,
                bestTime = float.MaxValue,
                highestScore = 0,
                levelFinished = false,
                levelUnlocked = (i == 1) // Solo el primer nivel desbloqueado.
            };

            GameData.levels.Add(levelData);
        }
        SaveGame(); // Guarda los datos inicializados.
    }

    public void StartLevel(int levelNumber)
    {
        // Busca datos del nivel.
        levelData = gameData.levels.Find(l => l.levelNumber == levelNumber);
        if(levelData != null)
        {
            Debug.Log($"Cargando nivel {levelNumber}: Mejor tiempo: {levelData.bestTime}, Mejor puntuación: {levelData.highestScore}");
        }
        else
        {
            Debug.Log($"Iniciando nivel {levelNumber}: Sin datos previos.");
        }
    }

    public void CompleteLevel(int levelNumber, float timeTaken, int monstersKilled, int score)
    {
        LevelData levelData = gameData.levels.Find(l => l.levelNumber == levelNumber)
                           ?? new LevelData { levelNumber = levelNumber };

        // Actualiza datos del nivel.
        levelData.timeTaken = timeTaken;
        levelData.monstersKilled = monstersKilled;
        levelData.score = score;
        levelData.bestTime = Mathf.Min(levelData.bestTime, timeTaken);
        levelData.highestScore = Mathf.Max(levelData.highestScore, score);
        levelData.levelFinished = true;

        // Desbloquea el siguiente nivel.
        UnlockNextLevel(levelNumber);

        gameData.levelsCompleted = Mathf.Max(gameData.levelsCompleted, levelNumber);

        OnLevelProgressChanged?.Invoke();

        SaveGame();
    }

    private void UnlockNextLevel(int currentLevel)
    {
        LevelData nextLevel = gameData.levels.Find(l => l.levelNumber == currentLevel + 1);
        if(nextLevel != null)
        {
            nextLevel.levelUnlocked = true;
        }
    }

    public void UnlockAllLevels()
    {
        for(int i = 0; i < gameData.levels.Count; i++)
        {
            gameData.levels[i].levelUnlocked = true;
            gameData.levels[i].levelFinished = true; // Necesario? revisar
        }

        OnLevelProgressChanged?.Invoke();
        print("Todos los niveles desbloqueados!");
        SaveGame();
    }

    public void ResetAllLevels()
    {
        gameData.levels.Clear();
        InitializeLevels(TotalLevels);
        OnLevelProgressChanged?.Invoke();
    }

    public bool CheckIfAllLevelsUnlocked()
    {
        return !gameData.levels.Any(level => !level.levelUnlocked);
    }

    public void LoadLevelInfo(LevelInfoSO levelInfo)
    {
        if(levelInfo == null) return;

        currentLevelInfo = levelInfo;
        OnLevelChanged?.Invoke();
        StartLevel(currentLevelInfo.ID);
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

    #endregion

    #region ---- Events ----
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
    #endregion

    // Guardar al cerrar el juego
    private void OnApplicationQuit()
    {
        if(levelData != null && levelData.levelNumber != 0)
        {
            gameData.lastLevelPlayed = levelData.levelNumber;
        }

        if(levelData == null) gameData.lastLevelPlayed = 1;

        SaveGame();
    }
}
