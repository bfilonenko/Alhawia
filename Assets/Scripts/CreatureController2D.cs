using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class CreatureController2D : MonoBehaviour
{
    public LayerMask groundedLayerMask;
    public Transform groundedTransform;
    public float groundedWidth = 1f;
    public float groundedHeight = 1f;

    [SingleLayer]
    public int platformLabel;

    public float stepOffset = 1f;

    public float movementSmoothTime = 0.05f;
    public float airControlSmoothTime = 0.5f;

    public bool isFlying = false;

    public UnityEvent<Collision2D> onCollisionEnter2DEvent;
    public UnityEvent<Collision2D> onCollisionStay2DEvent;
    public UnityEvent<Collision2D> onCollisionExit2DEvent;

    public bool isDrawGizgos = true;

    private Rigidbody2D mainRigidbody2D;

    private readonly List<Collider2D> overlapedColliders = new();

    private Vector3 currentVelocity = Vector3.zero;

    private float defaultGravityScale;

    public void SetFlying()
    {
        if (isFlying)
        {
            return;
        }

        mainRigidbody2D.gravityScale = 0f;
        isFlying = true;
    }

    public void SetWalking()
    {
        if (!isFlying)
        {
            return;
        }

        mainRigidbody2D.gravityScale = defaultGravityScale;
        isFlying = false;
    }

    public bool IsGrounded()
    {
        if (groundedTransform)
        {
            Vector2 offsetToCorner = new(groundedWidth / 2f, groundedHeight / 2f);
            Vector2 position = new(groundedTransform.position.x, groundedTransform.position.y);

            return Physics2D.OverlapArea(position - offsetToCorner, position + offsetToCorner, groundedLayerMask);
        }
        return false;
    }

    public bool IsCollidingWithPlatform()
    {
        int overlapAmount = mainRigidbody2D.OverlapCollider(new ContactFilter2D().NoFilter(), overlapedColliders);

        for (int i = 0; i < overlapAmount; ++i)
        {
            if (overlapedColliders[i].gameObject.layer == platformLabel)
            {
                return true;
            }
        }

        return false;
    }

    public void Move(Vector2 motion)
    {
        Vector3 target;
        if (isFlying)
        {
            target = new Vector3(motion.x, motion.y, 0f);
        }
        else
        {
            target = new Vector3(motion.x, mainRigidbody2D.velocity.y, 0f);
        }

        float smoothTime;
        if (IsGrounded())
        {
            smoothTime = movementSmoothTime;
        }
        else
        {
            smoothTime = airControlSmoothTime;
        }

        mainRigidbody2D.velocity = Vector3.SmoothDamp(mainRigidbody2D.velocity, target, ref currentVelocity, smoothTime);
    }

    public void Jump(float force)
    {
        if (isFlying)
        {
            return;
        }
        mainRigidbody2D.velocity = new Vector2(mainRigidbody2D.velocity.x, force);
    }

    public void SlowDownJump()
    {
        if (isFlying)
        {
            return;
        }

        if (mainRigidbody2D.velocity.y > 0f)
        {
            mainRigidbody2D.velocity = new Vector2(mainRigidbody2D.velocity.x, mainRigidbody2D.velocity.y / 2f);
        }
    }


    private void Awake()
    {
        mainRigidbody2D = GetComponent<Rigidbody2D>();
        defaultGravityScale = mainRigidbody2D.gravityScale;
    }

    private void Start()
    {
        if (isFlying)
        {
            mainRigidbody2D.gravityScale = 0f;
        }
        else
        {
            mainRigidbody2D.gravityScale = defaultGravityScale;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        onCollisionEnter2DEvent.Invoke(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        onCollisionStay2DEvent.Invoke(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onCollisionExit2DEvent.Invoke(collision);
    }

    private void OnDrawGizmos()
    {
        if (!isDrawGizgos)
        {
            return;
        }

        if (groundedTransform)
        {
            Vector3 topLeftCorner = groundedTransform.position +
                Vector3.left * groundedWidth / 2f +
                Vector3.up * groundedHeight / 2f;
            Vector3 topRightCorner = topLeftCorner + Vector3.right * groundedWidth;
            Vector3 bottomRightCorner = topRightCorner + Vector3.down * groundedHeight;
            Vector3 bottomLeftCorner = bottomRightCorner + Vector3.left * groundedWidth;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(topLeftCorner, topRightCorner);
            Gizmos.DrawLine(topRightCorner, bottomRightCorner);
            Gizmos.DrawLine(bottomRightCorner, bottomLeftCorner);
            Gizmos.DrawLine(bottomLeftCorner, topLeftCorner);
        }
    }
}
