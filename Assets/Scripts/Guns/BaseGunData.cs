using UnityEngine;

public class BaseGunData : MonoBehaviour
{
    public Rigidbody2D ownerRigidbody;

    public float lifeTime = 10f;
    public float damage = 20f;
    public float enemyKnockBack = 100000f;

    public float shakeDirectImpulseAmplitude = 1f;
    public float shakeCommonImpulseAmplitude = 1f;
    public float shakeDuration = 1f;
}
