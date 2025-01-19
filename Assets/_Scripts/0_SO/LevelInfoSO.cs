using UnityEngine;

[CreateAssetMenu(fileName = "Nivel ", menuName = "Nivel/Crear nivel")]
public class LevelInfoSO : ScriptableObject
{
    [Header("Config")]
    public int ID; // Debe casar con el buildIndex para funcionar

    [Header("Level Time")]
    public float threeStarsTime; 
    public float twoStarsTime;
    public float oneStarTime;

    [Header("Level info")]
    public Vector2 startingPosition;
    public int monstersToKill;

    [Header("Audio info")]
    public int musicIndexToPlay; // Igual que el ID del nivel

    public int CalculateStars(float time)
    {
        if(time <= threeStarsTime) return 3; // 3 estrellas
        if(time <= twoStarsTime) return 2;   // 2 estrellas
        if(time <= oneStarTime) return 1;    // 1 estrella
        return 0;                            // 0 estrellas
    }
}
