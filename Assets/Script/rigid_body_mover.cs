using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rigid_body_mover : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardAccel = 10.0f, reverseAccel = 4f, maxSpeed = 50f, turnstrength = 180, jumpForce = 10f, gravityForce = 10f;

    [SerializeField] Text countdown;
    private float speedInput, turnInput;
    private bool onGround, lost, standStill, outOfTime = false;
    private Vector3 startPosition;
    Quaternion startRotation;
    public TextMesh loseText, winText;
    float currentTime = 0f, startingTime = 10;
    private bool startTimer = false;
    float x, y, z;

    void Start()
    {
        

        currentTime = startingTime;
        rb = GetComponent<Rigidbody>();
        startPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        //startRotation = new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z);
        startRotation = transform.rotation;
        loseText.text = "";
        winText.text = "";
        currentTime = startingTime;


        // Moves the GameObject using it's transform.
        //rb.isKinematic = true;
    }


    void backToSpawnCrash()
    {
        lost = true;
        loseText.text = "Looks like ya crashed - Give it another go";
        startTimer = false;
        countdown.text = startingTime.ToString();
    }

    void backToSpawnTime()
    {
        loseText.text = "Looks like ya ran out of time - Give it another go";
        startTimer = false;
        countdown.text = startingTime.ToString();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("road"))
        {
            onGround = true;
            countdown.text = startingTime.ToString();
        }
        if (collision.gameObject.name.Contains("Barrel")) //if a barrel is hit
        {
            backToSpawnCrash();
            //thats rough buddy
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name.Equals("road"))
        {
            onGround = false;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name.Equals("Start_box"))
        {
            startTimer = true;

        }

        if (collision.gameObject.name.Equals("Finish_box"))
        {
            startTimer = false;
            winText.text = "Well Done champ, onto the next stage";
        }
    }

    void clock()
    {
        if (startTimer)
        {
            currentTime -= 1 * Time.deltaTime;
            countdown.text = currentTime.ToString("0");

            if (currentTime <= 0)
            {
                currentTime = 0;
                outOfTime = true;
            }
        }
}

    void Update()
    {
        clock();
        turnInput = Input.GetAxis("Horizontal");
        if (onGround)
        {
            speedInput = 0f;
            if (Input.GetAxis("Vertical") > 0) //pressing up/down or w/s
            {
                speedInput = Input.GetAxis("Vertical") * forwardAccel * 1000f;
            }
            else if (Input.GetAxis("Vertical") < 0) //pressing up/down or w/s
            {
                speedInput = Input.GetAxis("Vertical") * reverseAccel * 1000f;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnstrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));
        }
        else // in air
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnstrength * Time.deltaTime, 0f));
            rb.AddForce(Vector3.down * gravityForce, ForceMode.Impulse);
        }

        if (outOfTime)
        {
            lost = true;
            backToSpawnTime();
        }

        if (rb.velocity.z <= 0)
        {
            standStill = true;
        }
        else
        {
            standStill = false;
        }

        if (standStill && lost)
        {
            gameObject.transform.rotation = startRotation;
            gameObject.transform.position = startPosition;
            lost = false;
            outOfTime = false;
        }

    }



    void FixedUpdate()
    {

        if (Mathf.Abs(speedInput) > 0 && !lost)
        {
            rb.AddForce(transform.forward * speedInput);
        }
        //rb.AddForce(transform.forward* forwardAccel);


    }
}
