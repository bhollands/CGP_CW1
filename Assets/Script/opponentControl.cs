using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;

public class opponentControl : MonoBehaviour
{
    [SerializeField] Text countdown;
    public NavMeshAgent agent;
    public GameObject end_point, player, race_start_box, race_end_box, raceWall;
    public TextMesh player_message, score, position;
    Quaternion startRotation;
    private Vector3 endPosition, originalPosition;
    float currentTime = 0f, startingTime = 3;
    int score_value;
    private bool close, countDownFinished, AgentAhead = false, playerWin = false, startTimer = false, showTimer = true, raceOver = false;//, opAtEnd = false;
    void Start()
    {
        endPosition = new Vector3(end_point.transform.position.x, end_point.transform.position.y, end_point.transform.position.z); // position to go to in the race
        originalPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z); //opponent starting position 
        startRotation = transform.rotation; //oppent staring 
        currentTime = startingTime; //set current time to starting time for countdown clock
        position.text = ""; //have race position empty
    }

    void OnTriggerEnter(Collider collision)
    {
        string name = collision.gameObject.name;
        if (name.Equals("raceEnd_box")) //if at end of the race
        {
            if (AgentAhead && !playerWin)//if agemt is ahead when collidering with end box
            {
                player_message.text = "Ayy too bad kid, if you want a rematch come and see me"; //show loss message
                playerWin = false; //the player has lost
                raceOver = true; //the race is over
                showTimer = false; //stop showing the race timer
                countdown.text = ""; //make the countdown text disapear
            }
            agent.SetDestination(originalPosition);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        string name = collision.gameObject.name;
        if (name.Equals("raceWall")) //if trigger a collision with the racewall
        {
            gameObject.transform.rotation = startRotation; //return ot original rotation so ready for another race
            gameObject.transform.position = originalPosition;
        }
    }


    void clock()
    {
        if (startTimer)
        {
            currentTime -= 1 * Time.deltaTime; // whatever the current time take away 1 every chnage in time
            if (showTimer) //if the timer wants to be seen
            {
                countdown.text = currentTime.ToString("0"); //show how long is left in whole numbers
            }

            if (currentTime <= 1) //if the timer hits 1
            {
                currentTime = 1; //current time stays at 1
                countDownFinished = true;//the courntdown has finished
                startTimer = false; //reset start timer so can be used again
                if (showTimer) // if show timer again 
                {
                    countdown.text = "GO!!!!!"; //show the go message
                }
            }
        }
    }

    void playerMessage(string message) //write a message to the player
    {
        player_message.text = message;
    }

    void clearPlayerMessage() //clear any player messages
    {
        player_message.text = "";
    }
    void playerWinCase(float distToEnd) //case for when the player wins
    {
        if (distToEnd < 6 && !AgentAhead && !raceOver) // if distance to end is below 6, the agent is not ahead and the race isnt over
        {
            player_message.text = "congrats kid, let me know if you want to race again I'll be waiting"; //message from other driver 
            
            countdown.text = ""; //remove the countdown
            playerWin = true; //the player has won the race
            raceOver = true; //the race is over
        }
    }
    void start_race() //starting the race
    {
        //startTimer = true;
        if (countDownFinished && !raceOver) //countdown is finished and the race hasnt already started
        {
            agent.SetDestination(endPosition); //set to agents destination to the end
            (raceWall.GetComponent(typeof(Collider)) as Collider).isTrigger = true; //be able to go through all when race start
            startTimer = false; // countdown has ended so reset start timer
            
        }
    }

    void startRaceCountdown()
    {
        float distToStart = Vector3.Distance(player.transform.position, race_start_box.transform.position); // get the player distance from the start
        print(distToStart);
        if (Input.GetKey(KeyCode.G) && distToStart < 3.5) //if player presses 'g' and is close to the start i.e next to yellow car
        {
            (raceWall.GetComponent(typeof(Collider)) as Collider).isTrigger = false; //be able to go through all when race start
            clearPlayerMessage();// clear all player messages
            countDownFinished = false; //reset if player wants to go again
            raceOver = false; //the race isnt over
            currentTime = 5f; //length of race countdown
            startTimer = true; //start the timer
            showTimer = true; //show the timer
            playerWin = false; //the player has not won
        }
        start_race();
        
    }

    float getPlayerDistanceToEnd() //gets players distance from the end
    {
        float playerDistToEnd = Vector3.Distance(player.transform.position, race_end_box.transform.position);
        return playerDistToEnd;
    }

    float getOpponentDistanceToEnd() //get oppeneds distance to the end
    {
        float oppDistToEnd = Vector3.Distance(gameObject.transform.position, race_end_box.transform.position);
        return oppDistToEnd;
    }

    void determinPosition() //determin if player is first or second
    {
        if (countDownFinished && !raceOver) // during race
        {
            if (getPlayerDistanceToEnd() < getOpponentDistanceToEnd()) // if player is closer to end
            {
                AgentAhead = false; //they are first
                position.text = "1";
            }
            else
            {
                AgentAhead = true; //they are second
                position.text = "2";
            }

        }
    }
    void Update()
    {
        clock();
        startRaceCountdown();
        determinPosition();
        playerWinCase(getPlayerDistanceToEnd());
        if (Input.GetKey(KeyCode.LeftShift)) //clear player messages if annoying
        {
            clearPlayerMessage();
        }
        //move car to the position
    }
}
