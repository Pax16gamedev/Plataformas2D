using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] Transform levelContainer;
    [SerializeField] Button buttonPrefab;

    private SceneLoader loader;

    private void Start()
    {
        CreateButtons();

        loader = GameObject.FindGameObjectWithTag(Constants.TAGS.SCENE_LOADER)?.GetComponent<SceneLoader>();
    }

    private void CreateButtons()
    {
        var totalLevels = SceneManager.sceneCountInBuildSettings - 1; // Quito 1 por el menu principal

        for(int i = 1; i <= totalLevels; i++) // Empiezo por el 1 por el mismo motivo
        {
            var index = i; // Linea necesaria para cachear el valor. Lectura al entrar en select level
            Button btn = Instantiate(buttonPrefab, levelContainer);

            btn.onClick.AddListener(() => SelectLevel(index));
            btn.GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();

            // Mostrar progreso de estrellas
            //int stars = PlayerPrefs.GetInt("Nivel" + index + "_Estrellas", 0);
            //btn.transform.Find("StarsText").GetComponent<TextMeshProUGUI>().text = $"{stars} / 3";
        }
        print($"Total levels {totalLevels}");
    }

    public void SelectLevel(int nivelIndex)
    {
        //SceneManager.LoadScene(nivelIndex);
        loader.LoadSceneWithProgress(nivelIndex);
    }

    public void UpdateStars(int level, int stars)
    {
        // Guardar el progreso del jugador, por ejemplo:
        PlayerPrefs.SetInt("Nivel" + level + "_Estrellas", stars);
    }

    
}
