using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public void RunSFX(GameObject sfxPrefab, Transform point, float delay)
    {
        GameObject sfx = Instantiate(sfxPrefab, point.position, point.rotation);

        Destroy(sfx, sfx.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
    }
}
