using UnityEngine;

public class BaseBulletData : MonoBehaviour
{
    public AudioManager audioManager;
    public SFXManager sfxManager;
    public GameFreezer gameFreezer;

    public float lifeTime = 10f;
    public float damage = 20f;
    public float enemyKnockBack = 100000f;
    public float freezeTimeOnImpact = 0.01f;
}
