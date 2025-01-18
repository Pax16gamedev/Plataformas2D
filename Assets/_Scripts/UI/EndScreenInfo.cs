using TMPro;
using UnityEngine;

public class EndScreenInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI estrellasTMP;
    [SerializeField] TextMeshProUGUI timeTakenValueTMP;
    [SerializeField] TextMeshProUGUI bestTimeValueTMP;

    public void ShowInfo(int estrellas, float timeTaken, float bestTime)
    {
        estrellasTMP.text = estrellas.ToString();
        timeTakenValueTMP.text = $"{timeTaken:0.00}s";
        bestTimeValueTMP.text = $"{bestTime:0.00}s";
    }
}
