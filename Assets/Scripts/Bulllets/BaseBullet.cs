using UnityEngine;

[RequireComponent(typeof(BaseBulletParameters))]
public class BaseBullet : MonoBehaviour
{
    private BaseBulletParameters bulletParameters;


    private void Awake()
    {
        bulletParameters = GetComponent<BaseBulletParameters>();
    }

    private void Start()
    {
        Destroy(gameObject, bulletParameters.lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out HealthPoints healthPoints))
        {
            healthPoints.DealDamage(bulletParameters.damage);
        }

        Destroy(gameObject);
    }
}
