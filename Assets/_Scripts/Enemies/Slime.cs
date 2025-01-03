using UnityEngine;

public class Slime : EnemyBase
{
    private void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si atravieso al jugador
        if(collision.gameObject.CompareTag(Constants.TAGS.PLAYER_HITBOX))
        {
            collision.GetComponent<HealthSystem>()?.TakeDamage(bodyDamage);
        }

    }

}
