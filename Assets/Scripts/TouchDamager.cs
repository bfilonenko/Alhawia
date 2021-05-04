using UnityEngine;

public class TouchDamager : MonoBehaviour
{
    public LayerMask availableToTakeDamageMask;
    public float damage = 500f;


    private void HandleCollision(Collider2D collision)
    {
        if ((availableToTakeDamageMask.value & 1 << collision.gameObject.layer) != 0 &&
            collision.gameObject.TryGetComponent(out HealthPoints healthPoints))
        {
            healthPoints.DealDamage(damage);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        HandleCollision(collision);
    }
}
