using System.Collections;
using UnityEngine;

public class VisualDamageFeedback : MonoBehaviour
{
    [Header("Visual Feedback")]
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [Header("Particles")]
    [SerializeField] private ParticleSystem damageParticles;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip damageSound;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        originalColor = spriteRenderer.color;
    }

    public void TriggerFeedback()
    {
        if(spriteRenderer != null)
        {
            StartCoroutine(FlashColor());
        }

        if(damageParticles != null)
        {
            damageParticles.Play();
        }

        if(audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }
    }

    private IEnumerator FlashColor()
    {
        spriteRenderer.color = damageColor;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = originalColor;
    }
}
