using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flocking : MonoBehaviour
{

    public GameObject cube;
    public int numOfCubes;
    public GameObject[] allCubes;
    public Vector3 flyLimits = new Vector3(5, 5, 5);
    // Start is called before the first frame update

    [Header("Settings")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    public float distToNeibour;
    [Range(0.0f, 5.0f)]
    public float rotSpeed;

    void Start()
    {
        allCubes = new GameObject[numOfCubes];
        for (int i = 0; i < numOfCubes; i++)
        {
            Vector3 position = this.transform.position + new Vector3(Random.Range(-flyLimits.x, flyLimits.x), Random.Range(-flyLimits.y, flyLimits.y), Random.Range(-flyLimits.z, flyLimits.z));
            allCubes[i] = (GameObject)Instantiate(cube, position, Quaternion.identity);
            allCubes[i].GetComponent <cubeController>().myFlock = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
