using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHand : MonoBehaviour {

    // Inspector parameters
    //[Tooltip("The tracking device used for tracking the real hand.")]
    //public CommonTracker tracker;

    [Tooltip("The interactive used to represent the virtual hand.")]
    public Affect hand;

    [Tooltip("The button required to be pressed to grab objects.")]
    public CommonButton button;

    // Enumerate states of virtual hand interactions
    public enum MenuHandState
    {
        Open,
        Touching,
    };

    MenuHandState state;

    // Called at the end of the program initialization
    void Start()
    {
        state = MenuHandState.Open;
        // Ensure hand interactive is properly configured
        hand.type = AffectType.Virtual;
    }


    // FixedUpdate is not called every graphical frame but rather every physics frame
    void FixedUpdate()
    {

        // If state is open
        if (state == MenuHandState.Open)
        {

            // If the hand is touching something
            if (hand.triggerOngoing)
            {
                // Change state to touching
                state = MenuHandState.Touching;
            }
        }

        // If state is touching
        else if (state == MenuHandState.Touching)
        {
            //print("I am touching");
            // If the hand is not touching something
            if (!hand.triggerOngoing)
            {

                // Change state to open
                state = MenuHandState.Open;
            }

            // If the hand is touching something and the button is pressed
            else if (hand.triggerOngoing && button.GetPress())
            {

                // Fetch touched target
                GameObject go = hand.ongoingTriggers[0].gameObject;
                if (go.tag == "Button")
                {
                    SceneManager.LoadScene(go.name, LoadSceneMode.Single);
                }

            }
        }
    }


}
