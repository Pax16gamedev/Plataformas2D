using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private TrailRenderer trailRenderer;
    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void ActivateDashTrail() => trailRenderer.emitting = true;

    public void DeactivateDashTrail() => trailRenderer.emitting = false;
}
