using UnityEngine;

public class PositionFollower : MonoBehaviour
{
    public Transform followingTransform;


    private void Update()
    {
        transform.position = followingTransform.position;
    }
}
