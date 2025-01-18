using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthSlider;

    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    [SerializeField] TextMeshProUGUI healthTMP;

    private void Awake()
    {
        healthSlider = GetComponent<Slider>();
    }

    public void SetMaxHealth(float health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        health = Mathf.Clamp(health, 0, healthSlider.maxValue);
        healthSlider.value = health;
        healthTMP.text = health.ToString();
        
        fill.color = gradient.Evaluate(healthSlider.normalizedValue);
    }
}
