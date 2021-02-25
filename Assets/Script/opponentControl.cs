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
    private bool close, countDownFinished, AgentAhead = false, playerWin = false, startTimer = false, showTimer = true, raceOver = false, opReachedEnd = false;
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
                player_message.text = "You lost";
                playerWin = false;
                raceOver = true; 
                opReachedEnd = true;
                //agent.SetDestination(originalPosition);
            }
/*            else
            {
                agent.SetDestination(originalPosition);
            }*/
        }
        if (name.Equals("raceStartbox"))
        {
            opReachedEnd = false;
            gameObject.transform.rotation = startRotation;

        }
    }


    void clock()
    {
        if (startTimer)
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
                    countdown.text = "GO";
                }
            }
        }
    }

    //4 states in switch case
    //1. waiting to race()
    //2. racing()
    //3. Returning from race()

    void playerMessage(string message)
    {
        player_message.text = message;
    }

    void clearPlayerMessage()
    {
        player_message.text = "";
    }

    void positionCheck(float oppDistToEnd, float playerDistToEnd)
    {
        if (oppDistToEnd < playerDistToEnd)
        {
            AgentAhead = true;
        }
        else
        {
            AgentAhead = false;
        }
    }

    void playerWinCase(float distToEnd)
    {
        if (distToEnd < 6 && !AgentAhead && !playerWin)
        {
            player_message.text = "congrats kid, let me know if you want to race again";
            
            countdown.text = "";
            playerWin = true;
            raceOver = true;
        }
    }
    void start_race()
    {
        //startTimer = true;
        if (countDownFinished && !raceOver)
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
            clearPlayerMessage();
            countDownFinished = false; //reset if player wants to go again
            raceOver = false;
            currentTime = 3f;
            startTimer = true;
            playerWin = false;
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
    void Update()
    {
        clock();
        startRaceCountdown();  
        //float oppDistToEnd = Vector3.Distance(gameObject.transform.position, race_end_box.transform.position);
        //float playerDistToEnd = Vector3.Distance(player.transform.position, race_end_box.transform.position);

        positionCheck(getOpponentDistanceToEnd(), getPlayerDistanceToEnd());

        playerWinCase(getPlayerDistanceToEnd());
        if (countDownFinished && !raceOver)
        {
            if (getPlayerDistanceToEnd() < getOpponentDistanceToEnd()) //and the race isnt over
            {
                position.text = "1";
            }
            else
            {
                position.text = "2";
            }
            
        }

        if (raceOver)
        {
            agent.SetDestination(originalPosition);

        }

        //move car to the position
    }
}
