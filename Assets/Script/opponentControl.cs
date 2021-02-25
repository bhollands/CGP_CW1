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
    public GameObject end_point, player, race_start_box, race_end_box;
    public TextMesh player_message, score;
    Quaternion startRotation;
    private Vector3 endPosition, originalPosition;
    float currentTime = 0f, startingTime = 3;//, distBetweenCars;
    int scores;
    private bool close, countDownFinished, AgentAhead = false, playerWin = false, startTimer = false, showTimer = true, raceOver = false, opReachedEnd = false;
    void Start()
    {
        endPosition = new Vector3(end_point.transform.position.x, end_point.transform.position.y, end_point.transform.position.z); // position to go to
        originalPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        startRotation = transform.rotation;
        currentTime = startingTime;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name.Equals("raceEnd_box"))
        {
            if (AgentAhead && !playerWin)//if ahead when collidering with end box
            {
                player_message.text = "You lost";
                playerWin = false;
                raceOver = true; 
                opReachedEnd = true;
                agent.SetDestination(originalPosition);
            }
            else
            {
                agent.SetDestination(originalPosition);
            }
        }
        if (collision.gameObject.name.Equals("raceStartbox"))
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
    //4. Returning from race()

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
            float distBetweenCars = Vector3.Distance(player.transform.position, gameObject.transform.position); //distance between the two cars
            player_message.text = "congrats kid, let me know if you want to race again";
            
            countdown.text = "";
            playerWin = true;
            raceOver = true;
            scores = Int32.Parse(score.text);
            scores = scores + (int)distBetweenCars;

            score.text = scores.ToString();
        }
    }
    void start_race()
    {
        //startTimer = true;
        if (countDownFinished)
        {
            agent.SetDestination(endPosition);
            startTimer = false;
            countDownFinished = false;
        }
    }

    void startRaceCountdown()
    {
        float distToStart = Vector3.Distance(player.transform.position, race_start_box.transform.position);
        if ( distToStart < 3)
        {
            clearPlayerMessage();
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

    float getOpponentDistanceToEnd()
    {
        float oppDistToEnd = Vector3.Distance(gameObject.transform.position, race_end_box.transform.position);
        return oppDistToEnd;
    }
    void Update()
    {
        clock();
        if (Input.GetKey(KeyCode.G))
        {
            startRaceCountdown();
        }
        
        //float oppDistToEnd = Vector3.Distance(gameObject.transform.position, race_end_box.transform.position);
        //float playerDistToEnd = Vector3.Distance(player.transform.position, race_end_box.transform.position);

        positionCheck(getOpponentDistanceToEnd(), getPlayerDistanceToEnd());

        playerWinCase(getPlayerDistanceToEnd());

        //move car to the position
    }
}
