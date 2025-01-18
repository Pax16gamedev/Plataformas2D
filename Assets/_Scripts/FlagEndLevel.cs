using TMPro;
using UnityEngine;

public class FlagEndLevel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI statusTMP;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag(Constants.TAGS.PLAYER_HITBOX)) return;

        // Si el jugador a matado a todos los bichos
        if(GameManager.Instance.CheckIfAllMobsAreKilled())
        {
            GameManager.Instance.LevelCompleted();
        }
    }

    public void UpdateMobsStatus()
    {
        statusTMP.text = $"{GameManager.Instance.GetMonstersKilled()}/{GameManager.Instance.GetMonstersToKill()}";
    }

    private void Start()
    {
        UpdateMobsStatus();
    }
}
