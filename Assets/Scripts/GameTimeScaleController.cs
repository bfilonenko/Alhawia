using System.Collections;
using UnityEngine;

public class GameTimeScaleController : MonoBehaviour
{
    public float defaultSustainTime = 2f;
    public float defaultDecay = 2f;
    public float defaultScale = 0.2f;


    private float previousTimeScale = 1f;
    private float previousFixedDeltaTime = 0.02f;

    private bool isSlowMotionCoroutineRunning = false;
    private Coroutine freezeCoroutineRunning;
    private bool isFreezeCoroutineRunning = false;


    public void Freeze(float duration)
    {
        if (isFreezeCoroutineRunning || isSlowMotionCoroutineRunning)
        {
            return;
        }
        freezeCoroutineRunning = StartCoroutine(FreezeCoroutine(duration));
    }

    public void SlowMotion(float sustainTime, float decay, float scale)
    {
        if (isFreezeCoroutineRunning)
        {
            StopCoroutine(freezeCoroutineRunning);
            isFreezeCoroutineRunning = false;
            Time.timeScale = previousTimeScale;
        }

        if (isSlowMotionCoroutineRunning)
        {
            return;
        }
        StartCoroutine(SlowMotionCoroutine(sustainTime, decay, scale));
    }

    public void DefaultSlowMotion()
    {
        SlowMotion(defaultSustainTime, defaultDecay, defaultScale);
    }


    private IEnumerator FreezeCoroutine(float duration)
    {
        isFreezeCoroutineRunning = true;

        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = previousTimeScale;

        isFreezeCoroutineRunning = false;
    }

    private IEnumerator SlowMotionCoroutine(float sustainTime, float decay, float scale)
    {
        isSlowMotionCoroutineRunning = true;

        previousTimeScale = Time.timeScale;
        previousFixedDeltaTime = Time.fixedDeltaTime;
        Time.timeScale = scale * previousTimeScale;
        Time.fixedDeltaTime = scale * previousFixedDeltaTime;

        yield return new WaitForSecondsRealtime(sustainTime);

        float elapsed = 0f;

        while (elapsed < decay)
        {
            float x = elapsed / decay;

            float smoothStart = x * x;
            float smoothStop = 1 - (1 - x) * (1 - x);

            float currentScale = scale + Mathf.Lerp(smoothStart, smoothStop, x) * (1 - scale);

            Time.timeScale = previousTimeScale * currentScale;
            Time.fixedDeltaTime = previousFixedDeltaTime * currentScale;

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        Time.timeScale = previousTimeScale;
        Time.fixedDeltaTime = previousFixedDeltaTime;

        isSlowMotionCoroutineRunning = false;
    }
}
