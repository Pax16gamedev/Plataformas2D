using UnityEditor;
using UnityEngine;

//[CreateAssetMenu(fileName = "GameManager2", menuName = "Singletons/GameManager")]
public class GameManagerSO //: ScriptableSingleton<GameManagerSO>
{
    //[Header("Game Data")]
    //private GameData gameData;
    //public int totalLevels;
    //public int currentLevel;

    //public void Initialize()
    //{
    //    LoadGame();
    //}

    //public void SaveGame()
    //{
    //    SaveSystem.SaveGame(gameData);
    //}

    //public void LoadGame()
    //{
    //    gameData = SaveSystem.LoadGame();
    //}

    //public void CompleteLevel(int levelNumber, float timeTaken, int monstersKilled, int score)
    //{
    //    LevelData levelData = gameData.levels.Find(l => l.levelNumber == levelNumber)
    //                       ?? new LevelData { levelNumber = levelNumber };

    //    // Actualiza datos del nivel.
    //    levelData.score = score;
    //    levelData.bestTime = Mathf.Min(levelData.bestTime, timeTaken);
    //    levelData.levelFinished = true;

    //    // Desbloquea el siguiente nivel.
    //    int nextLevel = levelNumber + 1;
    //    if(nextLevel <= totalLevels)
    //    {
    //        LevelData nextLevelData = gameData.levels.Find(l => l.levelNumber == nextLevel)
    //                                ?? new LevelData { levelNumber = nextLevel, levelUnlocked = true };
    //        gameData.levels.Add(nextLevelData);
    //    }

    //    SaveGame();
    //}
}
