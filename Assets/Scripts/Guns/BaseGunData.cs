using UnityEngine;

public class BaseGunData : MonoBehaviour
{
    public Rigidbody2D ownerRigidbody;
    public GameTimeScaleController gameFreezer;

    public Transform shellContainer;

    public float lifeTime = 10f;
    public float damage = 20f;
    public float enemyKnockBack = 100000f;
    public float freezeTimeOnImpact = 0.01f;

    public float playerKnockBack = 10000f;

    public int bulletAmount = 3;
    public float angleBetweenBullets = 5f;
    public float gunKnockbackMagnitude = 1f;
    public float gunKnockbackDuration = 1f;

    public float shakeDirectImpulseAmplitude = 1f;
    public float shakeCommonImpulseAmplitude = 1f;
    public float shakeDuration = 1f;


    private void Awake()
    {
        CommonUtils.CheckFieldNotNull(ownerRigidbody, "Owner Rigidbody");
        CommonUtils.CheckFieldNotNullAndTryToSet(ref gameFreezer, "Game Freezer");
    }
}
