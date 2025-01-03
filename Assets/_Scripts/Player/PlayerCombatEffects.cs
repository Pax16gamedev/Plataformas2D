using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class PlayerCombatEffects : MonoBehaviour
{
    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    public void Poison(float duration, float damagePerSecond)
    {
        StartCoroutine(PoisonEffect(duration, damagePerSecond));
    }

    private IEnumerator PoisonEffect(float duration, float damagePerSecond)
    {
        float timeElapsed = 0;

        while(timeElapsed < duration)
        {
            timeElapsed++;
            healthSystem.TakeDamage(damagePerSecond);
            yield return new WaitForSeconds(1f);
        }
    }
}
