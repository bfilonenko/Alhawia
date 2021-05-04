using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BaseBulletData))]
public class BaseBullet : MonoBehaviour
{
    public Sound impactSound;
    public GameObject impactPrefab;
    public Sound explosionSoud;
    public GameObject explosionPrefab;

    public UnityEvent onImpact;


    private BaseBulletData bulletData;


    private void Awake()
    {
        bulletData = GetComponent<BaseBulletData>();
    }

    private void Start()
    {
        Destroy(gameObject, bulletData.lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        onImpact.Invoke();
        if (impactSound)
        {
            bulletData.audioManager.PlaySound(impactSound);
        }
        if (impactPrefab)
        {
            Vector2 normal = collision.GetContact(0).normal;

            bulletData.sfxManager.RunSFX(
                impactPrefab,
                collision.GetContact(0).point,
                Quaternion.Euler(0f, 0f, Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg),
                0f);
        }
        if (explosionSoud)
        {
            bulletData.audioManager.PlaySound(explosionSoud);
        }
        if (explosionPrefab)
        {
            bulletData.sfxManager.RunSFX(explosionPrefab, transform.transform, 0f);
        }

        if (collision.gameObject.TryGetComponent(out HealthPoints healthPoints))
        {
            healthPoints.DealDamage(bulletData.damage);

            collision.rigidbody.AddForce(transform.right * bulletData.enemyKnockBack, ForceMode2D.Impulse);

            bulletData.gameFreezer.Freeze(bulletData.freezeTimeOnImpact);
        }

        Destroy(gameObject);
    }
}
