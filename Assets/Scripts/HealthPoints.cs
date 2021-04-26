using UnityEngine;
using UnityEngine.Events;

public class HealthPoints : MonoBehaviour
{
    public float maxHealth = 100f;

    public UnityEvent onDamage;
    public UnityEvent onDead;


    private float currentHealth = 0f;


    public float CurrentHealth()
    {
        return currentHealth;
    }

    public float CurrentHealthPercent()
    {
        return currentHealth / maxHealth;
    }

    public bool IsAlive()
    {
        return !Mathf.Approximately(currentHealth, 0f);
    }

    public void DealDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (IsAlive())
        {
            onDamage.Invoke();
        }
        else
        {
            onDead.Invoke();
        }
    }


    private void Start()
    {
        currentHealth = maxHealth;
    }
}
