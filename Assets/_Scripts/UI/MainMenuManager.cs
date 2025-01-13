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
        if (lastLevel == 0)
        {
            lastLevel = 1;
        }

        levelSelection.SelectLevel(lastLevel);
    }

    public void ShowMainPanel() => mainPanel.SetActive(true);
    public void HideMainPanel() => mainPanel.SetActive(false);
    public void ShowSelectionPanel() => selectLevelPanel.SetActive(true);
    public void HideSelectionPanel() => selectLevelPanel.SetActive(false);

    public void CheckWhichContainerToShow()
    {
        var allLevelsUnlocked = GameManager.Instance.CheckIfAllLevelsUnlocked();

        if(!allLevelsUnlocked)
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
    public void ShowUnlockAllLevelsContainer() => unlockLevelsContainer.SetActive(true);
    public void HideUnlockAllLevelsContainer() => unlockLevelsContainer.SetActive(false);
    public void ShowResetAllLevelsContainer() => resetLevelsContainer.SetActive(true);
    public void HideResetAllLevelsContainer() => resetLevelsContainer.SetActive(false);

    public void ShowOptionsPanel() => optionsPanel.SetActive(true);
    public void HideOptionsPanel() => optionsPanel.SetActive(false);

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
