using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(HealthPoints))]
[RequireComponent(typeof(CreatureController2D))]
public class CharacterMovement : MonoBehaviour
{
    public Animator animator;

    public float movementSpeed = 1000f;
    public float jumpForse = 3100f;
    public float littleJumpForse = 2000f;
    public float hangTime = 0.2f;
    public float jumpCoolDown = 0.1f;
    public float jumpBufferLength = 0.3f;

    [SingleLayer]
    public int ignoringPlatformPlayerLabel;

    public bool isFlying = false;

    private CreatureController2D creatureController;
    private PlayerMovementInputAction inputActions;
    private Vector2 movement2D = Vector2.zero;
    private float movement1D = 0f;

    private float hangCounter = 0f;
    private float jumpBufferCounter = 0f;
    private float jumpCounter = 0f;

    private int defaultPlayerLayer;

    private bool isGrounded = false;
    private bool isSupposedToJump = false;
    private bool isSupposedToLittleJump = false;

    private bool isJumpingDown = false;
    private bool isSupposedToStopJumpingDown = false;

    public void SetFlying()
    {
        if (isFlying)
        {
            return;
        }

        creatureController.SetFlying();
        isFlying = true;
    }

    public void SetWalking()
    {
        if (!isFlying)
        {
            return;
        }

        creatureController.SetWalking();
        isFlying = false;
    }


    private void Move1D(InputAction.CallbackContext context)
    {
        movement1D = context.ReadValue<float>();
    }

    private void Move2D(InputAction.CallbackContext context)
    {
        movement2D = context.ReadValue<Vector2>();
    }


    private void Jump(InputAction.CallbackContext _)
    {
        jumpBufferCounter = jumpBufferLength;
        isSupposedToJump = true;
    }

    private void SlowDownJump(InputAction.CallbackContext _)
    {
        if (isSupposedToJump)
        {
            isSupposedToLittleJump = true;
        }

        creatureController.SlowDownJump();
    }

    private void StartJumpingDown(InputAction.CallbackContext _)
    {
        gameObject.layer = ignoringPlatformPlayerLabel;
        isJumpingDown = true;
    }

    private void StopJumpingDown(InputAction.CallbackContext _)
    {
        isSupposedToStopJumpingDown = true;
    }


    private void Awake()
    {
        inputActions = new PlayerMovementInputAction();
        creatureController = GetComponent<CreatureController2D>();
        defaultPlayerLayer = gameObject.layer;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void Start()
    {
        inputActions.Player.Move1D.performed += Move1D;
        inputActions.Player.Move1D.canceled += Move1D;

        inputActions.Player.Move2D.performed += Move2D;
        inputActions.Player.Move2D.canceled += Move2D;

        inputActions.Player.Jump.performed += Jump;
        inputActions.Player.Jump.canceled += SlowDownJump;

        inputActions.Player.JumpDown.performed += StartJumpingDown;
        inputActions.Player.JumpDown.canceled += StopJumpingDown;

        inputActions.Player.FlySwitcher.performed += context =>
        {
            if (isFlying)
            {
                SetWalking();
            }
            else
            {
                SetFlying();
            }
        };
    }

    private void Update()
    {
        jumpCounter -= Time.deltaTime;
        jumpBufferCounter -= Time.deltaTime;

        isGrounded = creatureController.IsGrounded();

        if (isGrounded && jumpCounter < 0f)
        {
            hangCounter = hangTime;
        }
        else
        {
            hangCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter < 0f)
        {
            isSupposedToJump = false;
            isSupposedToLittleJump = false;
        }

        if (jumpCounter < 0f &&
            hangCounter > 0f &&
            jumpBufferCounter > 0f &&
            !isJumpingDown)
        {
            if (isSupposedToLittleJump)
            {
                creatureController.Jump(littleJumpForse);
            }
            else
            {
                creatureController.Jump(jumpForse);
            }
            hangCounter = 0f;
            jumpBufferCounter = 0f;

            jumpCounter = jumpCoolDown;

            isSupposedToJump = false;
            isSupposedToLittleJump = false;
        }

        if (isJumpingDown && isSupposedToStopJumpingDown && !creatureController.IsCollidingWithPlatform())
        {
            gameObject.layer = defaultPlayerLayer;

            isJumpingDown = false;
            isSupposedToStopJumpingDown = false;
        }

        Vector2 movement;
        if (isFlying)
        {
            movement = movement2D;
            animator.SetFloat("Speed", 0f);
        }
        else
        {
            movement = Vector2.right * movement1D;
            animator.SetFloat("Speed", Mathf.Abs(movement1D));
        }

        if (!creatureController.IsFlipLocked())
        {
            if (movement.x > Mathf.Epsilon)
            {
                creatureController.Flip(true);
            }
            if (movement.x < -Mathf.Epsilon)
            {
                creatureController.Flip(false);
            }
        }

        creatureController.Move(movement * movementSpeed);
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
