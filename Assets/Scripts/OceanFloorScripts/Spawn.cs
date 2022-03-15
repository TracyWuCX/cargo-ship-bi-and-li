using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn: MonoBehaviour
{
    public GameObject Item;
    public LayerMask Player, Ground;

    [Header("=== Spawn Area ===")]
    private float xRange;
    private float yRange;
    private float zRange;
    public GameObject Area;
    public int distanceFromEdge;

    [Header("=== Spawn Amount ===")]
    [SerializeField] private int pointAmount = 10;
    [SerializeField] private int memberAmount = 1;
    public int currentAmount;

    // Start is called before the first frame update
    private void Awake()
    {
        // Set position at the middle of the area
        xRange = Area.transform.position.x;
        yRange = Area.transform.position.y;
        zRange = Area.transform.position.z;
        this.transform.position = Area.transform.position;
        
        for (int i = 0; i < pointAmount; i++)
        {
            float randomX = Random.Range(distanceFromEdge, xRange-distanceFromEdge);
            float randomY = Random.Range(distanceFromEdge, yRange-distanceFromEdge);
            float randomZ = Random.Range(distanceFromEdge, zRange-distanceFromEdge);
            Vector3 spawnPoint = new Vector3(randomX, randomY, randomZ);

            // Check for terrain (true when no ground below)
            if (!Physics.Raycast(spawnPoint, -transform.up, yRange, Ground))
            {
                 i--;
                 continue;
            }
            for (int j = 0; j < memberAmount; j++)
            {
                GameObject temp = Instantiate(Item, spawnPoint, Quaternion.identity);
                temp.transform.SetParent(this.transform);
            }
        }

        currentAmount = pointAmount * memberAmount;
    }

    // Update is called once per frame
    private void Update()
    {
        currentAmount = transform.childCount;
    }
}
