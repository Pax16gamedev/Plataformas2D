using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState { PLAYING, PAUSED }
    public GameState gameState = GameState.PLAYING;

    [SerializeField] bool debugMode = false;

    [Header("Game Data")]
    [SerializeField] private LevelInfoSO currentLevelInfo; // Serializado solo para modo editor
    private GameData gameData;
    private LevelData levelData;

    [Header("Level State")]
    private float timeElapsed = 0f;
    private int score = 0;
    private bool levelStarted = false;
    private bool levelFinished = false;
    private int monstersKilled = 0;
    private FlagEndLevel flagEndLevel;

    public GameData GameData => gameData;
    public bool LevelStarted => levelStarted;
    public bool LevelFinished => levelFinished;
    public int TotalLevels => SceneManager.sceneCountInBuildSettings - 1; // Excluyo el menu principal.

    public event Action OnLevelChanged; // El nivel ha cambiado
    public event Action OnLevelProgressChanged; // El progreso del nivel ha cambiado
    public event Action OnScoreChanged;

    private Player player;

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

#if UNITY_WEBGL && !UNITY_EDITOR
        Application.wantsToQuit += OnWebGLWantsToQuit;
#endif
    }

    // Se llama desde UIManager cada vez que se cargue una escena
    public void CheckForSceneGameObjects()
    {

        GameObject playerGO = GameObject.FindGameObjectWithTag(Constants.TAGS.PLAYER_HITBOX);
        player = playerGO.GetComponent<Player>();

        if(!debugMode)
        {
            player.transform.position = currentLevelInfo.startingPosition;
        }

        GameObject flagEndLevelGO = GameObject.FindGameObjectWithTag(Constants.TAGS.FLAG_END_LEVEL);
        flagEndLevel = flagEndLevelGO.GetComponent<FlagEndLevel>();
    }

    public void IncreaseScore(int scoreToAdd = 0)
    {
        if (scoreToAdd < 0) scoreToAdd = 0;

        score += scoreToAdd;
        OnScoreChanged?.Invoke();
        UIManager.Instance.UpdateScoreText(score);
    }

    public void IncreaseMonstersKilled()
    {
        monstersKilled++;
        flagEndLevel.UpdateMobsStatus();
    }
    public int GetMonstersKilled() => monstersKilled;
    public int GetMonstersToKill() => currentLevelInfo.monstersToKill;

    public bool CheckIfAllMobsAreKilled()
    {
        return monstersKilled == currentLevelInfo.monstersToKill;
    }

    private void OnDestroy()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Application.wantsToQuit -= OnWebGLWantsToQuit;
#endif
    }

    private void Update()
    {
        if(levelFinished || !levelStarted || gameState == GameState.PAUSED || !UIManager.Instance) return;

        timeElapsed += Time.deltaTime;
        UIManager.Instance.UpdateTimeText(timeElapsed);
    }

    #region ---- Game Flow - Save/Load ----
    public void ResetVariables()
    {
        timeElapsed = 0;
        score = 0;
    }

    public void PauseGame()
    {
        CanPlayerMove(false);
        CanPlayerAttack(false);
        gameState = GameState.PAUSED;
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameState = GameState.PLAYING;
        CanPlayerMove(true);
        CanPlayerAttack(true);
    }

    public void RestartLevel()
    {
        SceneLoader.Instance.LoadSceneWithProgress(currentLevelInfo.ID);
    }

    public void LevelCompleted()
    {
        PauseGame();
        CompleteLevel(levelData.levelNumber, timeElapsed, score);
        UIManager.Instance.ShowLevelEndScreen(levelData.stars, timeElapsed, levelData.bestTime);
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(gameData);
    }

    public void LoadGame()
    {
        gameData = SaveSystem.LoadGame();
        LoadLevelInfo(currentLevelInfo);
    }

    public void GoToNextLevel()
    {
        if(CheckIfIsLastLevel())
        {
            print("Estoy en el ultimo nivel. No puedo cargar un siguiente");
            return;
        }
        ResetVariables();
        SceneLoader.Instance.LoadSceneWithProgress(currentLevelInfo.ID + 1);
        OnLevelChanged?.Invoke();
    }

    public bool CheckIfIsLastLevel()
    {
        return currentLevelInfo.ID == TotalLevels;
    }

    #endregion

    #region ---- Level Management ----
    private void LoadLevelDataIfNecessary()
    {
        // Verificar si es la primera vez que se juega y si los datos de los niveles necesitan inicializacion.
        if(IsFirstGameplay())
        {
            InitializeLevels(TotalLevels);
            OnLevelProgressChanged?.Invoke();
        }
    }
    private bool IsFirstGameplay()
    {
        return gameData == null || gameData.levels == null || gameData.levels.Count == 0;
    }

    private void InitializeLevels(int totalLevels)
    {
        print($"Inicializando {totalLevels} niveles");
        //gameData = new GameData();
        for(int i = 1; i <= totalLevels; i++)
        {
            if(gameData.levels.Exists(l => l.levelNumber == i)) continue; // Evitar duplicados.

            LevelData levelData = new LevelData
            {
                levelNumber = i,
                score = 0,
                stars = 0,
                bestTime = float.MaxValue,
                levelFinished = false,
                levelUnlocked = (i == 1) // Solo el primer nivel desbloqueado.
            };

            gameData.levels.Add(levelData);
        }
        SaveGame(); // Guarda los datos inicializados.
    }

    public void StartLevel(int levelNumber)
    {
        // Busca datos del nivel.
        levelData = gameData.levels.Find(l => l.levelNumber == levelNumber);
        if(levelData != null)
        {
            Debug.Log($"Cargando nivel {levelNumber}: Mejor tiempo: {levelData.bestTime}\nEstrellas obtenidas {levelData.stars}");
        }
        else
        {
            Debug.Log($"Iniciando nivel {levelNumber}: Sin datos previos.");
        }
    }

    public void CompleteLevel(int levelNumber, float timeTaken, int score)
    {
        if(levelData == null)
        {
            Debug.LogError($"Level data no configurado para el nivel {levelNumber}");
            return;
        }

        // Actualiza datos del nivel.
        levelData.score = score;
        levelData.bestTime = Mathf.Min(levelData.bestTime, timeTaken);
        levelData.levelFinished = true;
        levelData.stars = currentLevelInfo.CalculateStars(timeElapsed);

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

    public void CanStartLevel()
    {
        if(currentLevelInfo == null || currentLevelInfo.ID == 0) // Importante: ID 0 -> Menu principal
        {
            Debug.LogWarning("Current level info not found!");
            levelStarted = false;
            return;
        }

        if(currentLevelInfo.monstersToKill == 0)
        {
            Debug.LogWarning("Monsters to kill on currentLevelInfo not set");
        }

        levelStarted = true;
    }

    #endregion

    #region ---- Events ----
    private void OnEnable()
    {
        //OnLevelChanged += CheckCurrentLevelInfo;
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

    #region Exit Game methods
    // Guardar al cerrar el juego
    private void OnExitGame()
    {
        if(levelData != null && levelData.levelNumber != 0) // MainMenu es nivel 0
        {
            gameData.lastLevelPlayed = levelData.levelNumber;
        }
        else
        {
            // Si no hay datos del nivel o estas en el menu, guarda el nivel por defecto (1).
            gameData.lastLevelPlayed = 1;
        }

        SaveGame();
    }

    private void OnApplicationQuit()
    {
        OnExitGame();
    }

#if UNITY_WEBGL
    private bool OnWebGLWantsToQuit()
    {
        Debug.Log("WebGL application is about to quit. Saving game data...");
        OnExitGame();
        return true; // Permitir que la aplicación se cierre.
    }
#endif
    #endregion

    #region Player flags
    public void CanPlayerMove(bool canMove) => player.CanMove(canMove);
    public void CanPlayerAttack(bool canAttack) => player.CanAttack(canAttack);

    #endregion

}
