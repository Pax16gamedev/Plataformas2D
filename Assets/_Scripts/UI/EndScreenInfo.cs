using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenInfo : MonoBehaviour
{
    [Header("Container")]
    [SerializeField] RectTransform buttonsContainer;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI estrellasTMP;
    [SerializeField] TextMeshProUGUI timeTakenValueTMP;
    [SerializeField] TextMeshProUGUI bestTimeValueTMP;

    [Header("Button")]
    [SerializeField] Button nextLevelBtn;
    [SerializeField] Button mainMenuBtn;

    [Header("Button config")]
    [SerializeField] float mainMenuBtnWidth = 320;
    [SerializeField] float mainMenuBtnHeight = 140;

    private void OnEnable()
    {
        if(GameManager.Instance.CheckIfIsLastLevel())
        {
            DeactivateNextLevelBtn();            
        }
    }

    public void ShowInfo(int estrellas, float timeTaken, float bestTime)
    {
        estrellasTMP.text = estrellas.ToString();
        timeTakenValueTMP.text = $"{timeTaken:0.00}s";
        bestTimeValueTMP.text = $"{bestTime:0.00}s";
    }

    public void DeactivateNextLevelBtn()
    {
        if(GameManager.Instance.CheckIfIsLastLevel())
        {
            nextLevelBtn.gameObject.SetActive(false);

            RectTransform buttonRect = mainMenuBtn.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0.5f);  // Anclar al centro del contenedor
            buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
            buttonRect.pivot = new Vector2(0.5f, 0.5f);       // Establecer el punto de pivote al centro
            buttonRect.anchoredPosition = Vector2.zero;       // Asegurar la posición en el centro
            buttonRect.sizeDelta = new Vector2(mainMenuBtnWidth, mainMenuBtnHeight); // Ancho x Alto
        }
    }
}
