using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;

[RequireComponent (typeof(Rigidbody))]

public class Boat : MonoBehaviour
{
    [Header("=== Movement Settings ===")]
    [SerializeField] private float yawTorque = 2000f; // spin left right
    [SerializeField] private float pitchTorque = 2000f; 
    //[SerializeField] private float rollTorque = 2000f;
    [SerializeField] private float thrust = 3000f;
    [SerializeField] private float upThrust = 2000f; // vertical up down
    [SerializeField] private float strafeThrust = 2000f; // horizontal left right

    [Header("=== Boosting Settings ===")]
    [SerializeField] private float maxBoostAmount = 100f; // energy amount
    [SerializeField] private float boostDeprecationRate = 0.25f; // energy decrease
    [SerializeField] private float boostRechargeRate = 0.5f;
    [SerializeField] private float boostMultiplier = 3f; // how fast
    // Bug fixing
    public bool boosting = false;
    public float currentBoostAmount;

    [Header("=== Glide Reduction Settings ===")]
    [SerializeField, Range(0.001f, 0.999f)] private float thrustGlideReduction = 0.111f;
    [SerializeField, Range(0.001f, 0.999f)] private float upDownGlideReduction = 0.111f;
    [SerializeField, Range(0.001f, 0.999f)] private float leftRightGlideReduction = 0.111f;
    // Bug fixing
    public float glide, verticalGlide, horizontalGlide = 0f;

    [Header("=== Shooting Settings ===")]
    public GameObject bullet;
    // References
    public Camera fpsCam;
    public Transform attackPoint;
    // Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;
    // Bullet force
    [SerializeField] private float shootForce = 300f;
    [SerializeField] private float upwardForce = 0f;
    //[SerializeField] private float recoilForce = 0f;
    // Gun states
    [SerializeField] private float timeBetweenShooting = 0.1f;
    [SerializeField] private float spread = 0f;
    [SerializeField] private float reloadTime = 1.5f;
    [SerializeField] private float timeBetweenShots = 0f;
    [SerializeField] private int magazineSize = 100;
    [SerializeField] private int bulletsPertap = 1;
    [SerializeField] private bool allowInvoke = true;
    // Bug fixing
    public bool allowButtonHold = true;
    public int bulletsLeft, bulletsShot;
    public bool shooting, readyToShoot, reloading;

    //[Header("=== Cameras Settings ===")]

    Rigidbody rb;
    private float thrust1D;
    private float strafe1D;
    private float upDown1D;
    //private float roll1D;
    private Vector2 pitchYaw;

    // Start is called before the first frame update
    private void Start()
    {
        // Cursor lock
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Boosting
        currentBoostAmount = maxBoostAmount;
        // Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        // Shooting
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        HandleMovement();
        HandleBoosting();
        HandleShooting();
        //HandleCameras();

        if (ammunitionDisplay != null)
        {
            ammunitionDisplay.SetText(bulletsLeft / bulletsPertap + "/" + magazineSize / bulletsPertap);
        }
    }

    private void HandleMovement()
    {
        //// Roll
        //rb.AddRelativeTorque(Vector3.back * roll1D * rollTorque * Time.deltaTime);
        // Pitch
        rb.AddRelativeTorque(Vector3.right * Mathf.Clamp(-pitchYaw.y, -1f, 1f) * pitchTorque * Time.deltaTime);
        // Yaw
        rb.AddRelativeTorque(Vector3.up * Mathf.Clamp(pitchYaw.x, -1f, 1f) * yawTorque * Time.deltaTime);

        // Thrust
        if (thrust1D > 0.1f || thrust1D < -0.1f) // controller conditions
        {
            float currentThrust;

            if (boosting)
            {
                currentThrust = thrust * boostMultiplier;
            }
            else
            {
                currentThrust = thrust;
            }

            rb.AddRelativeForce(Vector3.forward * thrust1D * currentThrust * Time.deltaTime);
            glide = thrust;
        }
        else
        {
            rb.AddRelativeForce(Vector3.forward * glide * Time.deltaTime);
            glide *= thrustGlideReduction;
        }

        // Up Down
        if (upDown1D > 0.1f || upDown1D < -0.1f) // controller conditions
        {
            rb.AddRelativeForce(Vector3.up * upDown1D * upThrust * Time.fixedDeltaTime);
            verticalGlide = upDown1D * upThrust;
        }
        else
        {
            rb.AddRelativeForce(Vector3.up * verticalGlide * Time.fixedDeltaTime);
            verticalGlide *= upDownGlideReduction;
        }

        // Strafe
        if (strafe1D > 0.1f || strafe1D < -0.1f) // controller conditions
        {
            rb.AddRelativeForce(Vector3.right * strafe1D * upThrust * Time.fixedDeltaTime);
            horizontalGlide = strafe1D * strafeThrust;
        }
        else
        {
            rb.AddRelativeForce(Vector3.right * horizontalGlide * Time.fixedDeltaTime);
            horizontalGlide *= leftRightGlideReduction;
        }

    }

    private void HandleBoosting()
    {
        if (boosting && currentBoostAmount > 0f)
        {
            currentBoostAmount -= boostDeprecationRate;
            if (currentBoostAmount <= 0f)
            {
                boosting = false;
            }
        }
        else
        {
            if (currentBoostAmount < maxBoostAmount)
            {
                currentBoostAmount += boostRechargeRate;
            }
        }
    }

    private void HandleShooting()
    {
        // Reloading
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            Reload();
        }
        // Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        // Find hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        // Check if hit
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75); // how long the bullet should disappear
        }
        // Direction
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
        // Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        // New direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
        // Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        // Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;
        // Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);
        //// Add recoil to player
        //rb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        // Instantiate muzzle flash
        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        }

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting); // calls function after "timeBetweenShooting" seconds
            allowInvoke = false;
        }

        // if more than one bulletsPerTap
        if (bulletsShot < bulletsPertap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    //void HandleCameras()
    //{

    //}

    #region Input Methods

    public void OnThrust(InputAction.CallbackContext context)
    {
        thrust1D = context.ReadValue<float>();
    }

    public void OnStrafe(InputAction.CallbackContext context)
    {
        strafe1D = context.ReadValue<float>();
    }

    public void OnUpDown(InputAction.CallbackContext context)
    {
        upDown1D = context.ReadValue<float>();
    }

    //public void OnRoll(InputAction.CallbackContext context)
    //{
    //    roll1D = context.ReadValue<float>();
    //}

    public void OnPitchYaw(InputAction.CallbackContext context)
    {
        pitchYaw = context.ReadValue<Vector2>();
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        boosting = context.performed;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        shooting = context.performed;
    }

    #endregion
}
