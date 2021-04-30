using UnityEngine;

public class GunVerticalAlignment : MonoBehaviour
{
    public Transform following;
    public Rigidbody2D playerRigidbody;

    public float maxOffset = 1f;
    public float frequency = 10f;
    public float dampingRatio = 0.5f;
    public float maxSpeed = 200f;


    private float currentHeight;
    private float currentSpeed = 0f;

    private void Awake()
    {
        currentHeight = following.position.y;

        CommonUtils.CheckFieldNotNull(playerRigidbody, "Player Rigidbody");
    }

    private void Update()
    {
        Vector3 position = transform.position;

        float followingHeight = following.position.y;

        float acceleration = -frequency * (currentHeight - followingHeight) - (currentSpeed * dampingRatio);

        currentSpeed += acceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        currentHeight += currentSpeed * Time.deltaTime;
        currentHeight = Mathf.Clamp(currentHeight, followingHeight - maxOffset, followingHeight + maxOffset);

        position.y = currentHeight;
        transform.position = position;
    }
}
