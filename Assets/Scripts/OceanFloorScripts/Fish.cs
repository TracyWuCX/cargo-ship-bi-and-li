using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fish : MonoBehaviour
{
    [Header("=== Enemy Health Settings ===")]
    public int currentHealth;

    [Header("=== Enemy Movement Settings ===")]
    private GameObject boat;
    private Transform player;
    public LayerMask Ground, Player;
    public bool playerInSight;
    Vector3 destination;
    bool destinationSet;
    public float patrolingRange;
    public float runningRange;
    public float patrolingSpeed;
    public float runningSpeed;

    // Start is called before the first frame update
    private void Start()
    {

    }

    private void Awake()
    {
        boat = FindPlayer();
        player = boat.GetComponent<Transform>();
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
        Action(playerInSight);
    }

    private void Action(bool isRunning)
    {
        if (isRunning)
        {
            //runningSpeed;

        }
        else
        {
            //patrolingSpeed;
        }
        
        if (!destinationSet)
        {
            SearchDestination(isRunning);
        }
        if (destinationSet)
        {
            //SetDestination(destination);
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
        float range = 0;
        if (isRunning)
        {
            range = runningRange;
        }
        else
        {
            range = patrolingRange;
        }
        float randomX = Random.Range(-range, range);
        //float randomY = Random.Range(-range, range);
        float randomZ = Random.Range(-range, range);
        destination = new Vector3(transform.position.x + randomX, transform.position.y + 0, transform.position.z + randomZ);

        // Check if it's in map, return true if hits
        if (Physics.Raycast(destination, -transform.up, 2f, Ground))
        {
            destinationSet = true; // destination is underground
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, runningRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, patrolingRange);
    }
}
