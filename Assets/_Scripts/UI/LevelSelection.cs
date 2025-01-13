using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Transform levelContainer;
    [SerializeField] Button buttonPrefab;

    private SceneLoader loader;

    private void Start()
    {
        InitializeComponents();
        // CreateOrReplaceButtons(); // Se invoca desde GameManager a traves de Evento
    }

    private void InitializeComponents()
    {
        // Buscar el cargador de escenas por etiqueta.
        loader = GameObject.FindGameObjectWithTag(Constants.TAGS.SCENE_LOADER)?.GetComponent<SceneLoader>();
        if(loader == null)
        {
            Debug.LogWarning("SceneLoader no encontrado en la escena.");
        }
    }

    private void CreateOrReplaceButtons()
    {
        print("Creando o reemplazando botones");
        for(int i = levelContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(levelContainer.GetChild(i).gameObject);
        }

        for(int i = 1; i <= GameManager.Instance.TotalLevels; i++) // Empiezo en 1 porque el indice 0 es el menu principal
        {
            var index = i; // Linea necesaria para cachear el valor. Lectura al entrar en select level
            Button btn = Instantiate(buttonPrefab, levelContainer);

            btn.onClick.AddListener(() => SelectLevel(index));
            btn.GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();

            DisplayLevelProgress(btn, index);
        }
    }

    private void DisplayLevelProgress(Button button, int levelIndex)
    {
        // Obtener datos del nivel desde el GameManager.
        LevelData levelData = GameManager.Instance.GameData.levels.Find(l => l.levelNumber == levelIndex);
        if(levelData == null) return;

        // Mostrar progreso de estrellas
        //int stars = PlayerPrefs.GetInt("Nivel" + index + "_Estrellas", 0);
        //btn.transform.Find("StarsText").GetComponent<TextMeshProUGUI>().text = $"{stars} / 3";

        // Mostrar el progreso de estrellas o cualquier otra informacion.
        //TextMeshProUGUI starsText = button.transform.Find("StarsText")?.GetComponent<TextMeshProUGUI>();
        //if(starsText != null)
        //{
        //    starsText.text = $"{levelData.highestScore} puntos"; // Ejemplo: Mostrar puntuacion mas alta.
        //}

        // Habilitar o deshabilitar el boton segun el estado del nivel.
        button.interactable = levelData.levelUnlocked;
    }

    public void SelectLevel(int nivelIndex)
    {
        //SceneManager.LoadScene(nivelIndex);
        loader.LoadSceneWithProgress(nivelIndex);
    }

    public void OnLevelProgressChanged()
    {
        CreateOrReplaceButtons();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnLevelProgressChanged += OnLevelProgressChanged;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnLevelProgressChanged -= OnLevelProgressChanged;
    }
}
