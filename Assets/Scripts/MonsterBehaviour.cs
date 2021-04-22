using UnityEngine;

[RequireComponent(typeof(CreatureController2D))]
public class MonsterBehaviour : MonoBehaviour
{
    private enum Direction1D : byte
    {
        None,
        Right,
        Left
    }


    public float movementSpeed = 1000f;
    public float jumpForse = 3100f;
    public float littleJumpForse = 2000f;
    public float jumpCoolDown = 0.1f;

    [SingleLayer]
    public int ignoringPlatformPlayerLabel;

    public bool isFlying = false;
    public bool isMovingFromSideToSide = false;
    public bool isFallingFromPlatform = false;


    private CreatureController2D creatureController;

    private Vector2 currentTarget = Vector2.zero;
    private Direction1D currentDirection1D = Direction1D.None;

    private void Awake()
    {
        creatureController = GetComponent<CreatureController2D>();
    }

    private void Start()
    {
        if (isMovingFromSideToSide)
        {
            currentTarget = Vector2.right;
            currentDirection1D = Direction1D.Right;
        }
    }

    private void Update()
    {
        if (isMovingFromSideToSide)
        {
            if (currentDirection1D == Direction1D.Right)
            {
                if ((!isFallingFromPlatform && creatureController.IsRightEndOfGround()) ||
                    (creatureController.IsRightWall()))
                {
                    currentDirection1D = Direction1D.Left;
                    currentTarget = Vector2.left;
                }
            }
            else if (currentDirection1D == Direction1D.Left)
            {
                if ((!isFallingFromPlatform && creatureController.IsLeftEndOfGround()) ||
                    (creatureController.IsLeftWall()))
                {
                    currentDirection1D = Direction1D.Right;
                    currentTarget = Vector2.right;
                }
            }

            creatureController.Move(currentTarget * movementSpeed);
        }
    }
}
