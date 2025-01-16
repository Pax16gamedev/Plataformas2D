using TMPro;
using UnityEngine;

public class LevelButtonInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nivelTMP;
    [SerializeField] private TextMeshProUGUI estrellasTMP;
    [SerializeField] private TextMeshProUGUI mejorTiempoTMP;

    public TextMeshProUGUI NivelTMP { get => nivelTMP; set => nivelTMP = value; }
    public TextMeshProUGUI EstrellasTMP { get => estrellasTMP; set => estrellasTMP = value; }
    public TextMeshProUGUI MejorTiempoTMP { get => mejorTiempoTMP; set => mejorTiempoTMP = value; }
}
