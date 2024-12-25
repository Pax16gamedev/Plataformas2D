using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] float fireForce = 8;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.AddForce(transform.right * fireForce, ForceMode2D.Impulse);
    }
}
