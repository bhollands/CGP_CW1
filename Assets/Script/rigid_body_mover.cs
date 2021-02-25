﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rigid_body_mover : MonoBehaviour
{
    public Rigidbody playerRB;
    public float forwardAccel = 10.0f, reverseAccel = 4f, maxSpeed = 50f, turnstrength = 180, jumpForce = 10f, gravityForce = 10f; //physics values

    [SerializeField] Text countdown;
    private float speedInput;//, turnInput;
    private bool onGround, lost;//, standStill;
    private Vector3 startPosition;
    Quaternion originalRotation;
    public TextMesh player_text, score;
    float currentTime = 0f, startingTime = 20;
    private bool startTimer = false;

    private int score_value = 0;
    
    void Start()
    {
        currentTime = startingTime;
        playerRB = GetComponent<Rigidbody>();
        startPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        originalRotation = transform.rotation;
        //player_text.text = "";
        clearPlayerMessage();
        score.text = score_value.ToString();
        currentTime = startingTime;
    }

    void lostToCrash()
    {
        lost = true;
        playerMessage("Looks like ya crashed - Give it another go");
        startTimer = false;
        currentTime = startingTime;
        //ResetPlayer(checkIfStationary()); //check it not moving before sending back
    }



    void OnCollisionEnter(Collision collision)
    {
        string name = collision.gameObject.name;
        if (name.Equals("road"))
        {
            onGround = true;
        }
        if (name.Contains("Barrel")) //if a barrel is hit
        {
            lostToCrash();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        string collisionName = collision.gameObject.name;
        if (collisionName.Equals("road"))
        {
            onGround = false;
        }
    }

    void playerMessage(string message)
    {
        player_text.text = message;
    }

    void clearPlayerMessage()
    {
        player_text.text = "";
    }

    void OnTriggerEnter(Collider collision)
    {
        string name = collision.gameObject.name;
        if (name.Equals("Start_box"))
        {
            startTimer = true;
            clearPlayerMessage();
            //player_text.text = "";

        }

        if (name.Equals("Finish_box"))
        {
            startTimer = false;
            playerMessage("Well Done champ, onto the next stage");
            countdown.text = "";
            score_value = score_value + 5;
        }

        if (Equals("raceInformant"))
        {
            player_text.text = "OK hotshot, lets see how you handle a race";
            player_text.alignment = TextAlignment.Center;
        }

        if (name.Equals("raceInfo"))
        {
            player_text.text = " Pull up next to the yellow car and press 'G' to start race countdown";
        }

    }

    bool timeOut()
    {
        if (startTimer)
        {
            currentTime -= 1 *Time.deltaTime;
            countdown.text = currentTime.ToString("0");

            if (currentTime <= 0)
            {
                currentTime = 0;
                return true;
            }
        }
        return false;
}


    void updateScore()
    {
        score.text = score_value.ToString("0");
    }


    void playerControl()
    {
        float turnInput = Input.GetAxis("Horizontal");
        float forwardBack = Input.GetAxis("Vertical");
        if (onGround)
        {
            speedInput = 0f;
            if (forwardBack > 0) //pressing up/down or w/s
            {
                speedInput = forwardBack * forwardAccel * 1000f;
            }
            else if (forwardBack < 0) //pressing up/down or w/s
            {
                speedInput = forwardBack * reverseAccel * 1000f;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); //gravity applied as an impulse for jumping
            }
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnstrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));
        }
        else // in air
        {
            turnInAir(turnInput);
        }
    }
    void turnInAir(float turnInput)
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnstrength * Time.deltaTime, 0f));
        playerRB.AddForce(Vector3.down * gravityForce, ForceMode.Impulse);
    }

    bool checkIfStationary()
    {
        if (playerRB.velocity.z <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void lostToTime()
    {
        lost = true;
        playerMessage("Looks like ya ran out of time - Give it another go");
        startTimer = false; // timer not to go
        currentTime = startingTime; //make it 20 seconds again
    }


    void ResetPlayer(bool stationary, bool lose)
    {
        if (stationary && lose) //if stood still and player has lost
        {
            score_value = score_value - 5; //take away score
            gameObject.transform.rotation = originalRotation; //transform to start position
            gameObject.transform.position = startPosition; //transform to end position
            lost = false; //reset loss factor
        }
    }

    void Update()
    {
        playerControl(); //give player control

        if (timeOut()) //if the player has timedout
        {
            lostToTime(); //lost to time condition
        }

        ResetPlayer(checkIfStationary(), lost); //check it not moving before sending back
  
        updateScore();
    }



    void FixedUpdate()
    {

        if (Mathf.Abs(speedInput) > 0 && !lost)
        {
            playerRB.AddForce(transform.forward * speedInput);
        }
    }
}
