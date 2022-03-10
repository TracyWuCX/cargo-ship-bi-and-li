using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn: MonoBehaviour
{
    public GameObject fish;
    public int totalAmount;
    public int currentAmount;

    // Start is called before the first frame update
    private void Start()
    {
        Vector3 instantiatePoint;
        instantiatePoint.x = 3;
        instantiatePoint.y = 3;
        instantiatePoint.z = 3;

        for (int i = 0; i < totalAmount; i++)
        {
            GameObject temp = Instantiate(fish, instantiatePoint, Quaternion.identity);
            temp.transform.SetParent(this.transform);
        }

        currentAmount = totalAmount;
    }

    // Update is called once per frame
    private void Update()
    {
        currentAmount = transform.childCount;
    }
}
