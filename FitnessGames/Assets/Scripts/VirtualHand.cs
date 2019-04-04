/*
Copyright ©2017. The University of Texas at Dallas. All Rights Reserved. 

Permission to use, copy, modify, and distribute this software and its documentation for 
educational, research, and not-for-profit purposes, without fee and without a signed 
licensing agreement, is hereby granted, provided that the above copyright notice, this 
paragraph and the following two paragraphs appear in all copies, modifications, and 
distributions. 

Contact The Office of Technology Commercialization, The University of Texas at Dallas, 
800 W. Campbell Road (AD15), Richardson, Texas 75080-3021, (972) 883-4558, 
otc@utdallas.edu, https://research.utdallas.edu/otc for commercial licensing opportunities.

IN NO EVENT SHALL THE UNIVERSITY OF TEXAS AT DALLAS BE LIABLE TO ANY PARTY FOR DIRECT, 
INDIRECT, SPECIAL, INCIDENTAL, OR CONSEQUENTIAL DAMAGES, INCLUDING LOST PROFITS, ARISING 
OUT OF THE USE OF THIS SOFTWARE AND ITS DOCUMENTATION, EVEN IF THE UNIVERSITY OF TEXAS AT 
DALLAS HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

THE UNIVERSITY OF TEXAS AT DALLAS SPECIFICALLY DISCLAIMS ANY WARRANTIES, INCLUDING, BUT 
NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
PURPOSE. THE SOFTWARE AND ACCOMPANYING DOCUMENTATION, IF ANY, PROVIDED HEREUNDER IS 
PROVIDED "AS IS". THE UNIVERSITY OF TEXAS AT DALLAS HAS NO OBLIGATION TO PROVIDE 
MAINTENANCE, SUPPORT, UPDATES, ENHANCEMENTS, OR MODIFICATIONS.
*/

using UnityEngine;
using System.Collections;

public class VirtualHand : Affect {
	
	// Enumerate states of virtual hand interactions
	public enum VirtualHandState {
        Playing,
        OnMenu
	};

	//// Inspector parameters
	//[Tooltip("The tracking device used for tracking the real hand.")]
	//public CommonTracker tracker;

	//[Tooltip("The interactive used to represent the virtual hand.")]
	//public Affect hand;
    
    [Tooltip("Way to pause")]
    public CommonButton pauseButton;

    [Tooltip("Way to continue")]
    public CommonButton continueButton;
    
	// Private interaction variables
	VirtualHandState state;

    //// Called at the end of the program initialization
    //void Start () {
    //	// Set initial state to open
    //	state = VirtualHandState.Playing;
    //	// Ensure hand interactive is properly configured
    //	//hand.type = AffectType.Virtual;
    //}
    //private void Update()
    //{
    //    if (pauseButton.GetPressDown())
    //    {
    //        if (state == VirtualHandState.Playing)
    //        {
    //            state = VirtualHandState.OnMenu;
    //            GameManager.instance.GamePause();
    //        }

    //    }
    //    else if(continueButton.GetPressDown())
    //    {
    //    if (state == VirtualHandState.OnMenu)
    //        {
    //            state = VirtualHandState.Playing;
    //            GameManager.instance.GameContinue();
    //        }
    //    }
    //}
    protected override void OnTriggerEnter(Collider trigger)
    {

        // Update all the states
        OnTriggerUpdate();

        // Avoid self triggering
        if (trigger.gameObject != gameObject)
        {
            // Avoid non-interactives unless not required
            if (trigger.gameObject.GetComponent<Interactive>() != null || interactivesOnly == false)
            {
                // Update the current state value
                triggerEntered = true;
                // Keep track of the current trigger
                enteredTriggers.Add(trigger);
                FlyingObject fo = trigger.GetComponent<FlyingObject>();
                if (fo != null)
                {
                    // asteroid
                    if (fo.breakable)
                    {
                        fo.Explode();
                    }
                    // space ship
                    else
                    {
                        fo.Shine();
                    }
                    GameManager.instance.ChangeScore(fo.score);
                }
            }
        }
    }
    // FixedUpdate is not called every graphical frame but rather every physics frame
    //   void FixedUpdate ()
    //{
    //       if (hand.triggerEntered)
    //       {
    //           foreach (Collider c in hand.enteredTriggers)
    //           {
    //               FlyingObject fo = c.GetComponent<FlyingObject>();
    //               if (fo != null)
    //               {
    //                   if (fo.breakable)
    //                   {
    //                       fo.Explode();
    //                   } else
    //                   {
    //                       fo.Shine();
    //                   }
    //                   GameManager.instance.ChangeScore(fo.score);
    //                   break;
    //               }
    //           }
    //           //print("get 1 score");
    //           //scoreSystem.AddScore(1);
    //       }
    //if (hand.triggerExited)
    //{
    //    print("leave");
    //}
    //       // If state is open
    //       if (state == VirtualHandState.Open) {

    //		// If the hand is touching something
    //		if (hand.triggerOngoing) {

    //			// Change state to touching
    //			state = VirtualHandState.Touching;
    //		}

    //		// Process current open state
    //		else {

    //			// Nothing to do for open
    //		}
    //	}

    //	// If state is touching
    //	else if (state == VirtualHandState.Touching) {
    //           //print("I am touching");
    //		// If the hand is not touching something
    //		if (!hand.triggerOngoing) {

    //			// Change state to open
    //			state = VirtualHandState.Open;
    //		}

    //		// If the hand is touching something and the button is pressed
    //		else if (hand.triggerOngoing && button.GetPress ()) {

    //			// Fetch touched target
    //			Collider target = hand.ongoingTriggers [0];
    //			// Create a fixed joint between the hand and the target
    //			grasp = target.gameObject.AddComponent<FixedJoint> ();
    //			// Set the connection
    //			grasp.connectedBody = hand.gameObject.GetComponent<Rigidbody> ();

    //			// Change state to holding
    //			state = VirtualHandState.Holding;
    //		}

    //		// Process current touching state
    //		else {

    //			// Nothing to do for touching
    //		}
    //	}

    //	// If state is holding
    //	else if (state == VirtualHandState.Holding) {

    //		// If grasp has been broken
    //		if (grasp == null) {

    //			// Update state to open
    //			state = VirtualHandState.Open;
    //		}

    //		// If button has been released and grasp still exists
    //		else if (!button.GetPress () && grasp != null) {

    //			// Get rigidbody of grasped target
    //			Rigidbody target = grasp.GetComponent<Rigidbody> ();
    //			// Break grasp
    //			DestroyImmediate (grasp);

    //			// Apply physics to target in the event of attempting to throw it
    //			target.velocity = hand.velocity * speed;
    //			target.angularVelocity = hand.angularVelocity * speed;

    //			// Update state to open
    //			state = VirtualHandState.Open;
    //		}

    //		// Process current holding state
    //		else {

    //			// Nothing to do for holding
    //		}
    //	}
    //}
}