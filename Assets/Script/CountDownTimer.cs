using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{

    [SerializeField] Text countdown;
    public TextMesh winText;
    public TextMesh loseText;
    float currentTime = 0f, startingTime = 10;
    private bool startTimer = false;
    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        winText.text = "";
 
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
 
    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            currentTime -= 1 * Time.deltaTime;
            countdown.text = currentTime.ToString("0");

            if (currentTime <= 0)
            {
                currentTime = 0;
            }
        }

    }
}
