﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updateSpeed : MonoBehaviour
{

    public Rigidbody playerRB;
    public TextMesh spedometer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        float kph = playerRB.velocity.magnitude * 3.6f; //get m/s 
        spedometer.text = kph.ToString("0") + " Km/h";
    }

    void FixedUpdate()
    {
        Vector3 vel = playerRB.velocity;
       
    }
}
