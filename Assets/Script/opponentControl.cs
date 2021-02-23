using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class opponentControl : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject end_point, player, race_start_box, race_end_box;
    public TextMesh player_message;
    private Vector3 position;
    private bool close, startRace, AgentAhead = false, playerWin = false;
    void Start()
    {
        position = new Vector3(end_point.transform.position.x, end_point.transform.position.y, end_point.transform.position.z); // position to go to
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name.Equals("raceEnd_box"))
        {
            if (AgentAhead)//if ahead when collidering with end box
            {
                player_message.text = "You lost";
                playerWin = false;
            }
            else //player has won 
            {
                playerWin = true;
                player_message.text = "You won";
            }
        }
    }
        void Update()
    {
        //player shown "how can you handel a race"
        //wait until player has enteredthe race box

        //tell them to press enter to start count down to race
        //have 3 second coutdown timer for the race
        //race
        //if player wins agents becomes a follower.
        //go up ramp to see the crowding
        float distBetweenCars = Vector3.Distance(player.transform.position, gameObject.transform.position); //distance between the two cars
        print(distBetweenCars);
        
        Vector3 playerPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

        float distToStart = Vector3.Distance(player.transform.position, race_start_box.transform.position);
        float oppDistToEnd = Vector3.Distance(gameObject.transform.position, race_end_box.transform.position);
        float playerDistToEnd = Vector3.Distance(player.transform.position, race_end_box.transform.position);
        
 
        if (Input.GetKey(KeyCode.G) && distToStart < 3)
        {
            //print(startRace);
            player_message.text = "GO GO GO";
            agent.SetDestination(position);
        }

        if (playerWin)
        {
            agent.SetDestination(playerPosition);
        }

        if (oppDistToEnd < playerDistToEnd)
        {
            AgentAhead = true;
        }

        //move car to the position
    }
}
