using UnityEngine;
using UnityEngine.Events;

public class CorpseFinalizer : MonoBehaviour
{
    public UnityEvent onFinalize;


    private bool availableToFinalize = false;

    private bool isFinalized = false;


    public bool AvailableToFinalize { get => availableToFinalize; set => availableToFinalize = value; }


    public void FinalizeCorpse()
    {
        if (!isFinalized)
        {
            onFinalize.Invoke();
            isFinalized = true;
        }
    }


    private void OnDisable()
    {
        if (availableToFinalize && !isFinalized)
        {
            onFinalize.Invoke();
            isFinalized = true;
        }
    }
}
