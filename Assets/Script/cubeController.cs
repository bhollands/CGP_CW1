using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeController : MonoBehaviour
{
    public flocking myFlock;
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(myFlock.minSpeed, myFlock.maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        makeFlock();
        transform.Translate(0,0,Time.deltaTime*speed);
    }

    void makeFlock()
    {
        GameObject[] cubes;
        cubes = myFlock.allCubes;

        Vector3 avgCenter = Vector3.zero; //average center
        Vector3 avgAvoid = Vector3.zero; //average avoidance vector
        float globalSpeed = 0.01f; // global speed of the group
        float neighbourDist; //neibour distance
        int groupSize = 0; //number of cubes within the neibour distance

        foreach(GameObject cube in cubes)
        {
            if (cube != this.gameObject) // if not cube currently running code
            {
                neighbourDist = Vector3.Distance(cube.transform.position, this.transform.position); //distance between cube position and current position in the world
                if (neighbourDist <= myFlock.distToNeibour) //if distance is less than or equal to nieghbour distance
                {
                    avgCenter += cube.transform.position; //add the position to the average center
                    groupSize++;//increase the group size

                    if (neighbourDist < 1.0f) //if the distance to nieghbour is less than 1
                    {
                        avgAvoid = avgAvoid + (this.transform.position - cube.transform.position); //average avoid vector added plus the vector away from the cubes
                    }

                    cubeController anotherFlock = cube.GetComponent<cubeController>();
                    globalSpeed = globalSpeed + anotherFlock.speed;
                }
            }
        }

        if (groupSize > 0) //if group size greater than 0 i.e if cube is not in a group it will keep moving in staight line
        {
            avgCenter = avgCenter / groupSize; //find the center of the group
            speed = globalSpeed / groupSize; //find the speed of the group

            Vector3 direction = (avgCenter + avgAvoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myFlock.rotSpeed * Time.deltaTime);
        }

    }
}
