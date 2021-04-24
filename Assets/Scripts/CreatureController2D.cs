using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CreatureController2D : MonoBehaviour
{
    public LayerMask groundedLayerMask;
    public BoxCollider2D groundedCollider;
    public BoxCollider2D leftWallCollider;
    public BoxCollider2D rightWallCollider;
    public BoxCollider2D leftEndOfGroundCollider;
    public BoxCollider2D rightEndOfGroundCollider;

    [SingleLayer]
    public int platformLabel;

    public float stepOffset = 1f;

    public float movementSmoothTime = 0.05f;
    public float airControlSmoothTime = 0.5f;

    public bool isFlying = false;

    public bool isDrawGizgos = true;


    private Rigidbody2D mainRigidbody2D;

    private readonly List<Collider2D> overlapedColliders = new();

    private Vector3 currentVelocity = Vector3.zero;

    private float defaultGravityScale;

    private int lockFlipCounter = 0;

    private bool isLookingToRight = true;


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

    public void LockFlip()
    {
        ++lockFlipCounter;
    }

    public void UnlockFlip()
    {
        --lockFlipCounter;
    }

    public bool IsFlipLocked()
    {
        Debug.Assert(lockFlipCounter >= 0);
        return lockFlipCounter > 0;
    }

    public void Flip(bool toRight)
    {
        if (toRight)
        {
            transform.rotation = Quaternion.identity;
            isLookingToRight = true;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            isLookingToRight = false;
        }
    }

    public bool IsLookingToRight()
    {
        return isLookingToRight;
    }

    public bool IsGrounded()
    {
        if (groundedCollider)
        {
            return groundedCollider.IsTouchingLayers(groundedLayerMask);
        }
        return false;
    }

    public bool IsLeftWallRaw()
    {
        if (leftWallCollider)
        {
            return leftWallCollider.IsTouchingLayers(groundedLayerMask);
        }
        return false;
    }

    public bool IsRightWallRaw()
    {
        if (rightWallCollider)
        {
            return rightWallCollider.IsTouchingLayers(groundedLayerMask);
        }
        return false;
    }

    public bool IsLeftEndOfGroundRaw()
    {
        if (leftEndOfGroundCollider)
        {
            return !leftEndOfGroundCollider.IsTouchingLayers(groundedLayerMask);
        }
        return true;
    }

    public bool IsRightEndOfGroundRaw()
    {
        if (rightEndOfGroundCollider)
        {
            return !rightEndOfGroundCollider.IsTouchingLayers(groundedLayerMask);
        }
        return true;
    }

    public bool IsLeftWall()
    {
        if (isLookingToRight)
        {
            return IsLeftWallRaw();
        }
        else
        {
            return IsRightWallRaw();
        }
    }

    public bool IsRightWall()
    {
        if (isLookingToRight)
        {
            return IsRightWallRaw();
        }
        else
        {
            return IsLeftWallRaw();
        }
    }

    public bool IsLeftEndOfGround()
    {
        if (isLookingToRight)
        {
            return IsLeftEndOfGroundRaw();
        }
        else
        {
            return IsRightEndOfGroundRaw();
        }
    }

    public bool IsRightEndOfGround()
    {
        if (isLookingToRight)
        {
            return IsRightEndOfGroundRaw();
        }
        else
        {
            return IsLeftEndOfGroundRaw();
        }
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


    private void OnDrawGizmos()
    {
        if (!isDrawGizgos)
        {
            return;
        }
    }
}
