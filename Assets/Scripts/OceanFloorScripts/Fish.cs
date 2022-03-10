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
    public int maxHight;
    public bool playerInSight;
    Vector3 destination;
    bool destinationSet;
    public float patrolingRange;
    public float runningRange;
    public float patrolingSpeed;
    public float runningSpeed;
    float range;
    float speed;
    Vector3 oriPosition;
    bool isOutside;

    // Start is called before the first frame update
    private void Start()
    {
        oriPosition = transform.position;
    }

    private void Awake()
    {
        boat = FindPlayer();
        player = boat.GetComponent<Transform>();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleHealth();
        HandleMovement();
    }

    private GameObject FindPlayer()
    {
        GameObject[] obj = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]; //will return an array of all GameObjects in the scene
        foreach (GameObject o in obj)
        {
            // player layer is 3
            if (o.layer == 3 && o.CompareTag("Player"))
            {
                return o;
            }
        }
        return null;
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
        Action(playerInSight);
    }

    private void Action(bool isRunning)
    {
        if (isRunning)
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
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * 2);
        }
        else if (!destinationSet)
        {
            SearchDestination(isRunning);
        }
        else if (destinationSet)
        {
            MoveFish(destination, speed);
        }

        Vector3 distanceToWalkPoint = transform.position - destination;
        // if reach the point, set walkPointSet to false
        if (distanceToWalkPoint.magnitude < 1f)
        {
            destinationSet = false;
        }
    }

    private void SearchDestination(bool isRunning)
    {
        if (isRunning)
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

    private void MoveFish(Vector3 destination, float speed)
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
