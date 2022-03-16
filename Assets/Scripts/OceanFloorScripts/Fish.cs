using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private GameObject boat;
    private Transform player;
    public LayerMask Player, Ground, Boundary;

    [Header("=== Enemy Health Settings ===")]
    public int currentHealth;

    [Header("=== Enemy Movement Settings ===")]
    public bool playerInSight;
    public float sightRange;
    public float patrolingSpeed;
    public float runningSpeed;
    private Transform fish;
    private Vector3 velocity;
    private float speed;
    private float switchDirection = 3;
    private float curTime = 0;

    // Awake is called on all objects in the scene before any object's Start function is called.
    private void Awake()
    {

    }

    // Start is called before the first frame update
    private void Start()
    {
        fish = this.transform;
        SetVelocity();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        HandleHealth();
        HandleMovement();
    }

    private void HandleHealth()
    {
        if (currentHealth <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.05f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void HandleMovement()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, Player);
        // true = is running, false = is patroling
        if (playerInSight)
        {
            speed = runningSpeed;
        }
        else
        {
            speed = patrolingSpeed;
        }
        MoveFish();
        RotateFish();
        fish.GetComponent<Rigidbody>().velocity = velocity;
    }

    private void RotateFish()
    {
        if (velocity != Vector3.zero)
        {
            fish.rotation = Quaternion.Slerp(fish.rotation, Quaternion.LookRotation(velocity), speed * Time.deltaTime);
        }
    }

    private void MoveFish()
    {
        if (curTime < switchDirection)
        {
            curTime += 1 * Time.deltaTime;
        }
        else
        {
            SetVelocity();
            if (Random.value > .5)
            {
                switchDirection += Random.value;
            }
            else
            {
                switchDirection -= Random.value;
            }
            if (switchDirection < 1)
            {
                switchDirection = 1 + Random.value;
            }
            curTime = 0;
        }
    }

    private void SetVelocity()
    {
        if (Random.value > .5)
        {
            velocity.x = speed * speed * Random.value;
        }
        else
        {
            velocity.x = -speed * speed * Random.value;
        }
        if (Random.value > .5)
        {
            velocity.y = speed * speed * Random.value;
        }
        else
        {
            velocity.y = -speed * speed * Random.value;
        }
        if (Random.value > .5)
        {
            velocity.z = speed * speed * Random.value;
        }
        else
        {
            velocity.z = -speed * speed * Random.value;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        SetVelocity();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
