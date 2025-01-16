using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject selectLevelPanel;
    [SerializeField] private GameObject optionsPanel;

    [Header("Subpaneles")]
    [SerializeField] private GameObject unlockLevelsContainer;
    [SerializeField] private GameObject resetLevelsContainer;

    [Header("Selector de niveles")]
    [SerializeField] private LevelSelection levelSelection;

    private void Start()
    {
        CheckWhichContainerToShow();
    }

    public void PlayGame()
    {
        var lastLevel = GameManager.Instance.GameData.lastLevelPlayed;
        lastLevel = Mathf.Max(lastLevel, 1); // Aseguro que al menos sea el nivel 1

        levelSelection.SelectLevel(lastLevel);
    }

    private void TogglePanel(GameObject panel, bool isActive)
    {
        if(panel == null)
        {
            Debug.LogWarning("Panel no asignado.");
            return;
        }

        panel.SetActive(isActive);
    }

    public void ShowMainPanel() => TogglePanel(mainPanel, true);
    public void HideMainPanel() => TogglePanel(mainPanel, false);
    public void ShowSelectionPanel() => TogglePanel(selectLevelPanel, true);
    public void HideSelectionPanel() => TogglePanel(selectLevelPanel, false);
    public void ShowOptionsPanel() => TogglePanel(optionsPanel, true);
    public void HideOptionsPanel() => TogglePanel(optionsPanel, false);

    public void CheckWhichContainerToShow()
    {
        if(!GameManager.Instance.CheckIfAllLevelsUnlocked())
        {
            ShowUnlockAllLevelsContainer();
            HideResetAllLevelsContainer();
        }
        else
        {
            HideUnlockAllLevelsContainer();
            ShowResetAllLevelsContainer();
        }
    }
    public void ShowUnlockAllLevelsContainer() => TogglePanel(unlockLevelsContainer, true);
    public void HideUnlockAllLevelsContainer() => TogglePanel(unlockLevelsContainer, false);
    public void ShowResetAllLevelsContainer() => TogglePanel(resetLevelsContainer, true);
    public void HideResetAllLevelsContainer() => TogglePanel(resetLevelsContainer, false);


    public void UnlockAllLevels()
    {
        GameManager.Instance.UnlockAllLevels();
        CheckWhichContainerToShow();
    }

    public void ResetAllLevels()
    {
        GameManager.Instance.ResetAllLevels();
        CheckWhichContainerToShow();
    }
}
