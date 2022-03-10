using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn: MonoBehaviour
{
    public GameObject Item;
    public LayerMask Player, Ground;

    [Header("=== Spawn Area ===")]
    public bool manualSet = false;
    public float xRange;
    public float yRange;
    public float zRange;
    public GameObject Area;
    public int distanceFromEdge;

    [Header("=== Spawn Amount ===")]
    public int pointAmount;
    public int memberAmount;
    public int currentAmount;
    public bool isRandom = true;
    public bool isGroup = false;

    // Start is called before the first frame update
    private void Start()
    {
        if (manualSet && Area)
        {
            // Set position at the middle of the area
            transform.position = Area.transform.position;
            xRange = transform.position.x;
            xRange = transform.position.y;
            xRange = transform.position.z;
        }

        if (isRandom == true)
        {
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

                if (isGroup == false)
                {
                    memberAmount = 1;
                }
                for (int j = 0; j < memberAmount; j++)
                {
                    GameObject temp = Instantiate(Item, spawnPoint, Quaternion.identity);
                    temp.transform.SetParent(this.transform);
                }
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
