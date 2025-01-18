using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("UI Elements")]
    [SerializeField] GameObject main;
    [SerializeField] TextMeshProUGUI loadingText;
    //[SerializeField] Slider progressBar;

    [Header("Delay")]
    [SerializeField] float waitTimeDelayInSeconds = 1;

    [Header("Levels")]
    [SerializeField] LevelInfoSO[] levels; // Alineado con Build Index -1

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
        CheckLevelsWithBuildIndex();
        main.SetActive(false);
    }

    private void CheckLevelsWithBuildIndex()
    {
        var coincidenNiveles = levels.Length == GameManager.Instance.TotalLevels;
        if(coincidenNiveles) return;

        Debug.Log($"No coinciden los niveles en {name} y BuildIndex. Revisar!");
    }

    public void LoadSceneWithProgress(int sceneIndex)
    {
        Time.timeScale = 1;
        main.SetActive(true);

        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    private void LoadLevelData(int sceneIndex)
    {
        if(levels == null || sceneIndex - 1 >= levels.Length)
        {
            Debug.LogError($"Levels is null {levels} or Scene index {sceneIndex} is out of bounds for levels array.");
            return;
        }

        LevelInfoSO levelInfo = levels[sceneIndex - 1];
        GameManager.Instance.LoadLevelInfo(levelInfo);
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        // No activar la escena hasta que la carga este completa
        operation.allowSceneActivation = false;

        LoadLevelData(sceneIndex);

        while (!operation.isDone)
        {
            // Actualizamos la barra de progreso
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            //progressBar.value = progress;

            loadingText.text = $"Cargando... {(int)(progress * 100)}%";

            // Una vez que llegamos al 90%, permitimos la activacion de la escena
            if (operation.progress >= 0.9f)
            {
                loadingText.text = $"Completado 100%";

                yield return new WaitForSeconds(waitTimeDelayInSeconds);
                operation.allowSceneActivation = true;
            }            
            yield return null;
        }
        main.SetActive(false);
        GameManager.Instance.CanStartLevel();
    }
}
