using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(BaseExplosionData))]
public class BaseExplosion : MonoBehaviour
{
    public CinemachineImpulseSource cinemachineImpulseSource;


    private BaseExplosionData explosionData;


    private void HandleCollision(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out HealthPoints healthPoints))
        {
            healthPoints.DealDamage(explosionData.damage);

            // Optimaze it
            GameTimeScaleController gameFreezer = FindObjectOfType<GameTimeScaleController>();
            if (gameFreezer)
            {
                gameFreezer.Freeze(explosionData.freezeTimeOnImpact);
            }
            else
            {
                Debug.LogWarning("Game Freezer not found");
            }
        }
    }


    private void Awake()
    {
        explosionData = GetComponent<BaseExplosionData>();
    }

    private void Start()
    {
        if (cinemachineImpulseSource)
        {
            cinemachineImpulseSource.GenerateImpulse(explosionData.shakeImpulseAmplitude);
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
