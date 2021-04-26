using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BaseBulletData))]
public class BaseBullet : MonoBehaviour
{
    public Sound impactSound;
    public GameObject impactPrefab;

    public UnityEvent onImpact;


    private BaseBulletData bulletParameters;


    private void Awake()
    {
        bulletParameters = GetComponent<BaseBulletData>();
    }

    private void Start()
    {
        Destroy(gameObject, bulletParameters.lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        onImpact.Invoke();
        if (impactSound)
        {
            bulletParameters.audioManager.PlaySound(impactSound);
        }
        if (impactPrefab)
        {
            Vector2 normal = collision.GetContact(0).normal;

            bulletParameters.sfxManager.RunSFX(
                impactPrefab,
                collision.GetContact(0).point,
                Quaternion.Euler(0f, 0f, Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg),
                0f);
        }

        if (collision.gameObject.TryGetComponent(out HealthPoints healthPoints))
        {
            healthPoints.DealDamage(bulletParameters.damage);
        }

        Destroy(gameObject);
    }
}
