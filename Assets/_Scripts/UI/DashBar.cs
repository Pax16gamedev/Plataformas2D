using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
    private Slider dashSlider;

    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    private void Awake()
    {
        dashSlider = GetComponent<Slider>();
    }

    public void SetMaxDashDuration(float dashDuration)
    {
        dashSlider.maxValue = dashDuration;
        dashSlider.value = dashDuration;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetDash(float currentDashDuration)
    {
        dashSlider.value = currentDashDuration;
        fill.color = gradient.Evaluate(dashSlider.normalizedValue);
    }
}
