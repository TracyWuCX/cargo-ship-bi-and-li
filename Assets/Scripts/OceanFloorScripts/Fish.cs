using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fish : MonoBehaviour
{
    private GameObject boat;
    private Transform player;
    public LayerMask Player, Ground, Boundary;

    [Header("=== Enemy Health Settings ===")]
    public int currentHealth;

    [Header("=== Enemy Movement Settings ===")]
    public bool playerInSight;
    public float patrolingRange;
    public float runningRange;
    public float patrolingSpeed;
    public float runningSpeed;
    private Vector3 destination;
    private bool destinationSet;
    private float range;
    private float speed;
    private Vector3 oriPosition;
    private bool isOutside;

    // Awake is called on all objects in the scene before any object's Start function is called.
    private void Awake()
    {

    }

    // Start is called before the first frame update
    private void Start()
    {
        oriPosition = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleHealth();
        HandleMovement();
    }

    private void HandleHealth()
    {
        if (currentHealth <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void HandleMovement()
    {
        playerInSight = Physics.CheckSphere(transform.position, patrolingRange, Player);
        // true = is running, false = is patroling
        Action();
    }

    private void Action()
    {
        if (playerInSight)
        {
            speed = runningSpeed;

        }
        else
        {
            speed = patrolingSpeed;
        }

        if (isOutside)
        {
            destination = oriPosition;
            MoveFish();
        }
        else if (!destinationSet)
        {
            SearchDestination();
        }
        else if (destinationSet)
        {
            MoveFish();
        }

        Vector3 distanceToWalkPoint = transform.position - destination;
        // if reach the point, set walkPointSet to false
        if (distanceToWalkPoint.magnitude < 1f)
        {
            destinationSet = false;
        }
    }

    private void SearchDestination()
    {
        if (playerInSight)
        {
            range = runningRange;
        }
        else
        {
            range = patrolingRange;
        }
        float randomX = Random.Range(-range, range);
        float randomY = Random.Range(-range, range);
        float randomZ = Random.Range(-range, range);
        destination = new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z + randomZ);
        destinationSet = true;
    }

    private void MoveFish()
    {
        // Rotate model (random speed)
        float turnSpeed = speed * Random.Range(1f, 3f);
        Vector3 lookAt = destination - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAt), turnSpeed * Time.deltaTime);
        // Move
        transform.position = Vector3.MoveTowards(transform.position, destination, speed);
    }

    private void OnTriggerExit(Collider other)
    {
        isOutside = true;
    }

    private void OnTriggerStay(Collider other)
    {
        isOutside = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, runningRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, patrolingRange);
    }
}
