using System.Collections;
using UnityEngine;

public class GameFreezer : MonoBehaviour
{
    private bool isFreezeCoroutineRunning = false;

    public void Freeze(float duration)
    {
        if (isFreezeCoroutineRunning)
        {
            return;
        }
        StartCoroutine(FreezeCoroutine(duration));
    }


    private IEnumerator FreezeCoroutine(float duration)
    {
        isFreezeCoroutineRunning = true;

        float currentTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = currentTimeScale;

        isFreezeCoroutineRunning = false;
    }
}
