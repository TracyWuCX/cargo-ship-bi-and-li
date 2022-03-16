using UnityEngine;
using System.Collections;

public class FishTest : MonoBehaviour
{
    Transform stuff; // Needs rigidbody attached with a collider
    Vector3 vel; // Holds the random velocity
    float switchDirection= 3;
    float curTime= 0;
 
    void Start()
    {
        stuff = this.transform;
        SetVel();
    }

    void SetVel()
    {
        if (Random.value > .5)
        {
            vel.x = 10 * 10 * Random.value;
        }
        else
        {
            vel.x = -10 * 10 * Random.value;
        }
        if (Random.value > .5)
        {
            vel.z = 10 * 10 * Random.value;
        }
        else
        {
            vel.z = -10 * 10 * Random.value;
        }
    }

    void Update()
    {
        if (curTime < switchDirection)
        {
            curTime += 1 * Time.deltaTime;
        }
        else
        {
            SetVel();
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

    void FixedUpdate()
    {
        stuff.GetComponent<Rigidbody>().velocity = vel;
    }
}