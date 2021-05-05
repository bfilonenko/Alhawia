using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public Text displayText;


    private void Update()
    {
        int counter = (int) (1f / Time.unscaledDeltaTime);
        displayText.text = counter.ToString() + " FPS";
    }
}
