using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    [Tooltip("The interactive used to represent the virtual hand.")]
    public Affect hand;


    [Tooltip("Way to pause")]
    public CommonButton pauseButton;

    [Tooltip("button to skip training and return to menu")]
    public CommonButton skipButton;

    bool pauseButtonOnClick = false;
    bool skipButtonOnClick = false;
    bool pauseButtonDown = false;
    bool skipButtonDown = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        bool buttonPress = false;
        if (pauseButton.GetPressDown())
        {
            buttonPress = true;
            pauseButtonDown = true;
        }
        if (!buttonPress && pauseButtonDown)
        {
            pauseButtonOnClick = true;
            pauseButtonDown = false;
        }
        buttonPress = false;
        //TODO test skip+pause
        if (skipButton.GetPressDown())
        {
            buttonPress = true;
            skipButtonDown = true;
        }
        if (!buttonPress && skipButtonDown)
        {
            skipButtonOnClick = true;
            skipButtonDown = false;
        }
        if (pauseButtonOnClick)
        {
            //   print("click");
            pauseButtonOnClick = false;
            GameManager.instance.MenuOnClick();
        }
        else if (skipButtonOnClick)
        {
            skipButtonOnClick = false;
            GameManager.instance.TriggerOnClick();
        }
        
    }
}
