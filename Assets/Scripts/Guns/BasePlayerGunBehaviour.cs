using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BaseGunData))]
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
    public SFXManager sfxManager;

    public CinemachineImpulseSource cinemachineCommonImpulseSource;
    public CinemachineImpulseSource cinemachineDirectImpulseSource;

    public Transform firePoint;

    public GameObject bulletPrefab;

    public Sound shootSound;
    public GameObject shootSFXPrefab;

    public float inactiveTime = 1f;
    public float gunAlignmentSpeed = 1f;

    public float recoil = 0f;
    public float cooldown = 0.2f;
    public float scatter = 2f;

    public float bulletForce = 1000f;

    public MouseButtonToShoot mouseButtonToShoot = MouseButtonToShoot.Left;

    public CreatureController2D creatureController;

    public UnityEvent onShoot;


    [HideInInspector]
    public GunState gunState = GunState.Idle;


    private GunState previousGunState = GunState.Idle;

    private BaseGunData baseGunData;

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
        if (gunState != GunState.Shooting)
        {
            Debug.LogWarning("Fire button released without pressing");
            return;
        }
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
            if ((gunState == GunState.Shooting || gunState == GunState.Cessation)
                && !isLocked)
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
        baseGunData = GetComponent<BaseGunData>();

        CommonUtils.CheckMainCameraNotNullAndTryToSet(ref mainCamera);
        CommonUtils.CheckFieldNotNullAndTryToSet(ref audioManager, "Audio Manager");
        CommonUtils.CheckFieldNotNullAndTryToSet(ref sfxManager, "SFX Manager");
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

            Vector3 gunRotation = firePoint.eulerAngles;
            gunRotation.z += (Random.value - 0.5f) * scatter;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(gunRotation));
            Rigidbody2D bulletRigidbody2D = bullet.GetComponent<Rigidbody2D>();
            bulletRigidbody2D.AddForce(bullet.transform.right * bulletForce, ForceMode2D.Impulse);

            BaseBulletData baseBulletData = bullet.GetComponent<BaseBulletData>();
            CommonUtils.CopyBaseGunDataToBaseBulletData(baseGunData, baseBulletData);
            baseBulletData.audioManager = audioManager;
            baseBulletData.sfxManager = sfxManager;

            Vector2 playerKnockBack = -bullet.transform.right * baseGunData.playerKnockBack;
            playerKnockBack.y = Mathf.Max(0f, playerKnockBack.y);

            baseGunData.ownerRigidbody.AddForce(playerKnockBack, ForceMode2D.Impulse);

            if (cinemachineDirectImpulseSource)
            {
                cinemachineCommonImpulseSource.GenerateImpulse(baseGunData.shakeCommonImpulseAmplitude);
                cinemachineDirectImpulseSource.GenerateImpulse(firePoint.right * baseGunData.shakeDirectImpulseAmplitude);
            }
            onShoot.Invoke();
            if (shootSound)
            {
                audioManager.PlaySound(shootSound);
            }
            if (shootSFXPrefab)
            {
                sfxManager.RunSFX(shootSFXPrefab, firePoint, 0f);
            }
        }
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
