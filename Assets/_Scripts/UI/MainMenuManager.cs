using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject selectLevelPanel;
    [SerializeField] private GameObject optionsPanel;

    [Header("Selector de niveles")]
    [SerializeField] private LevelSelection levelSelection;

    public void PlayGame()
    {
        levelSelection.SelectLevel(1); // TODO:: Cambiar esto a ultimo nivel jugado
    }


    public void ShowMainPanel() => mainPanel.SetActive(true);
    public void HideMainPanel() => mainPanel.SetActive(false);
    public void ShowSelectionPanel() => selectLevelPanel.SetActive(true);
    public void HideSelectionPanel() => selectLevelPanel.SetActive(false);

    public void ShowOptionsPanel() => optionsPanel.SetActive(true);
    public void HideOptionsPanel() => optionsPanel.SetActive(false);
}
