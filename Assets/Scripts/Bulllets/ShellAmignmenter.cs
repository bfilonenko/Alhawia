using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShellAmignmenter : MonoBehaviour
{
    public float dumpingTime = 1f;
    public float distanceToFinishMovement = 10f;
    public float angleEpsilon = 2f;


    private Rigidbody2D shellRigidbody;

    private Vector3 previousPosition = Vector3.zero;

    private void CheckAngleAndTryToSet(float angle)
    {
        Vector3 eulerAnlge = new(0f, 0f, angle);

        if (Quaternion.Angle(transform.rotation, Quaternion.Euler(eulerAnlge)) < angleEpsilon)
        {
            transform.rotation = Quaternion.Euler(eulerAnlge);
        }
    }

    private void HandleAlignment()
    {
        if (shellRigidbody.IsSleeping())
        {
            return;
        }

        if ((transform.position - previousPosition).sqrMagnitude < distanceToFinishMovement * distanceToFinishMovement)
        {
            CheckAngleAndTryToSet(0f);
            CheckAngleAndTryToSet(90f);
            CheckAngleAndTryToSet(180f);
            CheckAngleAndTryToSet(270f);
            shellRigidbody.velocity = Vector2.zero;
            shellRigidbody.Sleep();
        }

        previousPosition = transform.position;
    }


    private void Awake()
    {
        shellRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(HandleAlignment), dumpingTime, dumpingTime);
    }
}
