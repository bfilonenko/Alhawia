using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BaseGunParameters))]
public class BasePlayerGunBehaviour : MonoBehaviour
{
    public enum GunState : byte
    {
        Idle,
        Shooting,
        Cessation,
        Alignment
    }

    public enum MouseButtonToShoot : byte
    {
        Left,
        Right
    }


    public Camera mainCamera;
    public AudioManager audioManager;

    public Transform firePoint;

    public GameObject bulletPrefab;

    public Sound shootSound;

    public float inactiveTime = 1f;
    public float gunAlignmentSpeed = 1f;

    public float recoil = 0f;
    public float cooldown = 0.2f;
    public float scatter = 2f;

    public float bulletForce = 1000f;

    public MouseButtonToShoot mouseButtonToShoot = MouseButtonToShoot.Left;

    public CreatureController2D creatureController;


    [HideInInspector]
    public GunState gunState = GunState.Idle;


    private GunState previousGunState = GunState.Idle;

    private BaseGunParameters baseGunParameters;

    private PlayerMouseInputAction inputActions;

    private Vector2 mousePosition = Vector2.zero;

    private float cessationTime = 0f;
    private float lastShootTime = 0f;

    private bool isLocked = false;


    private void StartShooting(InputAction.CallbackContext _)
    {
        gunState = GunState.Shooting;
    }

    private void StopShooting(InputAction.CallbackContext _)
    {
        gunState = GunState.Cessation;
        cessationTime = 0f;
    }

    private void HandleMousePosition(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    private void HandleGunState()
    {
        if (previousGunState != gunState)
        {
            if (gunState == GunState.Shooting && !isLocked)
            {
                isLocked = true;
                creatureController.LockFlip();
            }
            else if (gunState == GunState.Alignment)
            {
                isLocked = false;
                creatureController.UnlockFlip();
            }
        }

        previousGunState = gunState;

        if (gunState == GunState.Shooting)
        {
            Vector2 mouseWorldPosition =
                CommonUtils.GetMouseWorldPosition(mainCamera, mousePosition);

            Vector2 direction = mouseWorldPosition - new Vector2(transform.position.x, transform.position.y);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (transform.position.x < mouseWorldPosition.x)
            {
                creatureController.Flip(true);
                transform.localRotation = Quaternion.Euler(0f, 0f, angle);
            }
            else
            {
                creatureController.Flip(false);
                transform.localRotation = Quaternion.Euler(0f, 0f, 180f - angle);
            }
        }
        else if (gunState == GunState.Cessation)
        {
            if (cessationTime > inactiveTime)
            {
                gunState = GunState.Alignment;
            }

            cessationTime += Time.deltaTime;
        }
        else if (gunState == GunState.Alignment)
        {
            if (Quaternion.Angle(transform.localRotation, Quaternion.identity) < Mathf.Epsilon)
            {
                gunState = GunState.Idle;
            }
            else
            {
                transform.localRotation = Quaternion.Lerp(
                    transform.localRotation,
                    Quaternion.identity,
                    gunAlignmentSpeed * Time.deltaTime);
            }
        }
    }


    private void Awake()
    {
        inputActions = new PlayerMouseInputAction();
        baseGunParameters = GetComponent<BaseGunParameters>();

        CommonUtils.CheckMainCameraNotNullAndTryToSet(ref mainCamera);
        CommonUtils.CheckFieldNotNullAndTryToSet(ref audioManager, "Audio Manager");
        CommonUtils.CheckFieldNotNull(creatureController, "Creature Controller");
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void Start()
    {
        inputActions.Mouse.MousePosition.performed += HandleMousePosition;
        inputActions.Mouse.MousePosition.canceled += HandleMousePosition;

        if (mouseButtonToShoot == MouseButtonToShoot.Left)
        {
            inputActions.Mouse.LeftMouseFire.performed += StartShooting;
            inputActions.Mouse.LeftMouseFire.canceled += StopShooting;
        }
        else
        {
            inputActions.Mouse.RightMouseFire.performed += StartShooting;
            inputActions.Mouse.RightMouseFire.canceled += StopShooting;
        }

        inputActions.Mouse.EndlessShooting.performed += StartShooting;
    }

    private void Update()
    {
        HandleGunState();

        if (gunState == GunState.Shooting && Time.time - lastShootTime > cooldown)
        {
            lastShootTime = Time.time;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D bulletRigidbody2D = bullet.GetComponent<Rigidbody2D>();
            bulletRigidbody2D.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);

            BaseBulletParameters baseBulletParameters = bullet.GetComponent<BaseBulletParameters>();
            baseBulletParameters.damage = baseGunParameters.damage;

            if (shootSound)
            {
                audioManager.PlaySound(shootSound);
            }
        }
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
