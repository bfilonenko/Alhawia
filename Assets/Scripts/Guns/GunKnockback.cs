using System.Collections;
using UnityEngine;

public class GunKnockback : MonoBehaviour
{
    private Coroutine knockbackCoroutine;
    private bool isKnockbackCoroutineRunning = false;


    public void Knockback(float magnitude, float duration)
    {
        if (isKnockbackCoroutineRunning)
        {
            StopCoroutine(knockbackCoroutine);
        }

        knockbackCoroutine = StartCoroutine(KnockbackCoroutine(magnitude, duration));
    }

    private IEnumerator KnockbackCoroutine(float magnitude, float duration)
    {
        isKnockbackCoroutineRunning = true;

        Vector3 position = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = elapsed / duration;
            x = 1f - x;
            x = x * x * x;
            x = 1f - x;

            position.x = Mathf.Lerp(-magnitude, 0f, x);
            transform.localPosition = position;

            elapsed += Time.deltaTime;

            yield return null;
        }

        position.x = 0f;
        transform.localPosition = position;

        isKnockbackCoroutineRunning = false;
    }
}
