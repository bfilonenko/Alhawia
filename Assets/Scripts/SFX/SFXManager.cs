using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public void RunSFX(GameObject sfxPrefab, Vector3 point, Quaternion rotation, float delay)
    {
        GameObject sfx = Instantiate(sfxPrefab, point, rotation);

        Destroy(sfx, sfx.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
    }

    public void RunSFX(GameObject sfxPrefab, Transform point, float delay)
    {
        RunSFX(sfxPrefab, point.position, point.rotation, delay);
    }
}
