using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Transform levelContainer;
    [SerializeField] Button buttonPrefab;

    private void Start()
    {
        CreateOrReplaceButtons(); // Se invoca desde GameManager a traves de un Evento
    }

    private void CreateOrReplaceButtons()
    {
        ClearExistingButtons();

        foreach(LevelData levelData in GameManager.Instance.GameData.levels) 
        {
            //var index = i; // Linea necesaria para cachear el valor. Lectura al entrar en select level
            Button btn = Instantiate(buttonPrefab, levelContainer);
            LevelButtonInfo buttonInfo = btn.GetComponent<LevelButtonInfo>();

            ConfigureButton(btn, buttonInfo, levelData);

            btn.onClick.AddListener(() => SelectLevel(levelData.levelNumber));
        }
    }

    private void ClearExistingButtons()
    {
        for(int i = levelContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(levelContainer.GetChild(i).gameObject);
        }
    }

    private void ConfigureButton(Button button, LevelButtonInfo buttonInfo, LevelData levelData)
    {
        buttonInfo.NivelTMP.text = $"Nivel {levelData.levelNumber}";
        buttonInfo.EstrellasTMP.text = $"{levelData.stars} estrellas";

        string mejorTiempo = levelData.bestTime == float.MaxValue ? "-" : $"{levelData.bestTime:0.00}s";
        buttonInfo.MejorTiempoTMP.text = $"Mejor Tiempo: {mejorTiempo}";

        // Habilitar o deshabilitar el boton segun el estado del nivel.
        button.interactable = levelData.levelUnlocked;
    }

    public void SelectLevel(int nivelIndex)
    {
        //SceneManager.LoadScene(nivelIndex);
        SceneLoader.Instance.LoadSceneWithProgress(nivelIndex);
    }

    public void OnLevelProgressChanged()
    {
        CreateOrReplaceButtons();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnLevelProgressChanged += OnLevelProgressChanged;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnLevelProgressChanged -= OnLevelProgressChanged;
    }
}
