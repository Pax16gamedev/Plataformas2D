using System.Collections.Generic;

[System.Serializable]
public class LevelData
{
    public int levelNumber;
    public float timeTaken;
    public int score;
    public int monstersKilled;
    public float bestTime;
    public int highestScore;
    public bool levelFinished;
    public bool levelUnlocked;
}

[System.Serializable]
public class GameData
{
    public int levelsCompleted;
    public int lastLevelPlayed;
    public List<LevelData> levels = new List<LevelData>();
    public GameOptions options = new GameOptions();
    public string lastSaveDate;
}

[System.Serializable]
public class GameOptions
{
    public float volume;
}

