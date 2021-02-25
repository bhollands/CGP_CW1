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
    float currentTime = 0f, startingTime = 3;//, distBetweenCars;
    int score_value;
    private bool close, countDownFinished, AgentAhead = false, playerWin = false, startTimer = false, showTimer = true, raceOver = false;//, opAtEnd = false;
    void Start()
    {
        endPosition = new Vector3(end_point.transform.position.x, end_point.transform.position.y, end_point.transform.position.z); // position to go to
        originalPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        startRotation = transform.rotation;
        currentTime = startingTime;
        position.text = "";
    }

    void OnTriggerEnter(Collider collision)
    {
        string name = collision.gameObject.name;
        if (name.Equals("raceEnd_box"))
        {
            if (AgentAhead && !playerWin)//if ahead when collidering with end box
            {
                player_message.text = "Ayy too bad kid, if you want a rematch come and see me";
              
                playerWin = false;
                raceOver = true;
                //opAtEnd = true;
                showTimer = false;
                countdown.text = "";
            }
        }
        if (name.Equals("raceStartbox"))
        {
            //opAtEnd = false;
            //gameObject.transform.rotation = startRotation;

        }
    }

    void OnTriggerExit(Collider collision)
    {
        string name = collision.gameObject.name;
        if (name.Equals("raceWall"))
        {
            
            gameObject.transform.rotation = startRotation;

        }
    }


    void clock()
    {
        currentTime -= 1 * Time.deltaTime;
        if (showTimer)
        {
            countdown.text = currentTime.ToString("0");
        }
            
        if (currentTime <= 1)
        {
            currentTime = 1;
            countDownFinished = true;
            startTimer = false;
            if (showTimer)
            {
                countdown.text = "GO!!!!!";
            }
        }
    }

    void playerMessage(string message)
    {
        player_message.text = message;
    }

    void clearPlayerMessage()
    {
        player_message.text = "";
    }
    void playerWinCase(float distToEnd)
    {
        if (distToEnd < 6 && !AgentAhead && !raceOver)
        {
            player_message.text = "congrats kid, let me know if you want to race again I'll be waiting";
            
            countdown.text = "";
            playerWin = true;
            raceOver = true;
        }
    }
    void start_race()
    {
        //startTimer = true;
        if (countDownFinished && !raceOver) //countdown is finished and race is over
        {
            agent.SetDestination(endPosition);
            (raceWall.GetComponent(typeof(Collider)) as Collider).isTrigger = true; //be able to go through all when rcae start
            startTimer = false;
            
        }
    }

    void startRaceCountdown()
    {
        float distToStart = Vector3.Distance(player.transform.position, race_start_box.transform.position);
        if (Input.GetKey(KeyCode.G) && distToStart < 3)
        {
            (raceWall.GetComponent(typeof(Collider)) as Collider).isTrigger = false; //be able to go through all when rcae start
            clearPlayerMessage();
            countDownFinished = false; //reset if player wants to go again
            raceOver = false; //the race isnt over
            currentTime = 5f; //length of ravce countdown
            startTimer = true; //start the timer
            showTimer = true; //show the timer
            playerWin = false; //the player has not won
        }
        start_race();
    }

    float getPlayerDistanceToEnd()
    {
        float playerDistToEnd = Vector3.Distance(player.transform.position, race_end_box.transform.position);
        return playerDistToEnd;
    }

    void returnToStart()
    {
        agent.SetDestination(originalPosition);
    }

    float getOpponentDistanceToEnd()
    {
        float oppDistToEnd = Vector3.Distance(gameObject.transform.position, race_end_box.transform.position);
        return oppDistToEnd;
    }

    void determinPosition()
    {
        if (countDownFinished && !raceOver)
        {
            if (getPlayerDistanceToEnd() < getOpponentDistanceToEnd()) //and the race isnt over
            {
                AgentAhead = false;
                position.text = "1";
            }
            else
            {
                AgentAhead = true;
                position.text = "2";
            }

        }
    }
    void Update()
    {
        if (startTimer)
        {
            clock();
        }
        
        startRaceCountdown();

        determinPosition();
        //positionCheck(getOpponentDistanceToEnd(), getPlayerDistanceToEnd());

        playerWinCase(getPlayerDistanceToEnd());


        if (raceOver)
        {
            agent.SetDestination(originalPosition);

        }

        if (Input.GetKey(KeyCode.C))
        {
            clearPlayerMessage();
        }
        //move car to the position
    }
}
