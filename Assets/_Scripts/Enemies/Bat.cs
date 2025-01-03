using UnityEngine;

public class Bat : EnemyBase
{
    protected override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(Constants.TAGS.PLAYER_DETECTION))
        {
            print("Player detectado. Inmolación!!!!");
        }
        else if(collision.gameObject.CompareTag(Constants.TAGS.PLAYER_HITBOX))
        {
            collision.GetComponent<HealthSystem>()?.TakeDamage(bodyDamage);
        }
    }
}
