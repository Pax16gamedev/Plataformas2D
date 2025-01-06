using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Top Container")]
    [SerializeField] private TextMeshProUGUI timeTMP;
    [SerializeField] private TextMeshProUGUI scoreTMP;

    [Header("End Screen Panel")]
    [SerializeField] private GameObject levelEndScreenPanel;


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

    public void UpdateTimeText(float text)
    {
        timeTMP.text = $"tiempo: {text:F2} s";
    }

    public void UpdateScoreText(int score)
    {
        scoreTMP.text = $"Puntos: {score.ToString()}";
    }

    public void ShowLevelEndScreen(int stars, int score)
    {
        levelEndScreenPanel.SetActive(true);
        //starsText.text = $"Estrellas: {stars} / 3\n" +
        //    $"Puntuación: {score}";
    }
}
