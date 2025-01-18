using UnityEngine;

public class Golem_Stone : MonoBehaviour
{
    [SerializeField] Slime slimePrefab;
    [SerializeField] int slimesToSpawn = 2;
    [SerializeField] float timeBetweenAttacks = 2f;
    [SerializeField] int damage = 20;

    [SerializeField]
    Vector2[] directions = new Vector2[] // Direcciones predefinidas
    {
        new Vector2(1,1),
        new Vector2(-1,1)
    };

    private bool playerInRange = false;
    private Transform jugador;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDeath += Fragment;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            healthSystem.TakeDamage(50);
        }
    }

    private void Atacar()
    {
        if(!jugador || !playerInRange) return;

        // Danho directo si el jugador esta cerca del golem
        if(Vector2.Distance(transform.position, jugador.position) <= 2f)
        {
            jugador.GetComponent<HealthSystem>().TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Detection") && collision.CompareTag("Player"))
        {
            playerInRange = true;
            jugador = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Detection") && collision.CompareTag("Player"))
        {
            playerInRange = false;
            jugador = null;
        }
    }

    private void Fragment(HealthSystem enemySystem)
    {
        for(int i = 0; i < slimesToSpawn; i++)
        {
            Slime slime = Instantiate(slimePrefab, transform.position, Quaternion.identity);
            Vector2 direccion = directions[i % directions.Length];
            Rigidbody2D rb = slime.GetComponent<Rigidbody2D>();
            if(rb != null)
            {
                float slimeSpeed = 1.25f;
                rb.velocity = direccion * slimeSpeed; // Asigna velocidad al slime
            }
        }
    }

    private void OnDestroy()
    {
        healthSystem.OnDeath -= Fragment;
    }
}
