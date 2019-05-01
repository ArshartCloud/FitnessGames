using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class countDown : MonoBehaviour {

    float currentTime = 0f;
    float startingTime = 5f;

    [SerializeField] TextMeshPro countdownText;

    // Use this for initialization
    void Start()
    {
        //countdownText = GetComponent<TextMeshPro>();
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        countdownText.text = Mathf.RoundToInt(currentTime).ToString();

        // countdownText.text = currentTime.ToString("0");

        if(currentTime <= 0)
        {
            countdownText.text = "GO!";

            // Start the game here
        }
    }
}
