using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReclaimWall : Affect {
    protected override void OnTriggerExit(Collider trigger)
    {
        print("I got u");
        // Update all the states
        OnTriggerUpdate();
        // Avoid self triggering
        if (trigger.gameObject != gameObject)
        {
            // Avoid non-interactives unless not required
            if (trigger.gameObject.GetComponent<Interactive>() != null || interactivesOnly == false)
            {
                // Update the current state value
                triggerExited = true;
                // Keep track of the current trigger
                exitedTriggers.Add(trigger);



                FlyingObject fo = trigger.gameObject.GetComponent<FlyingObject>();
                if (fo != null)
                {
                    if (fo.state == FlyingObject.FlyingObjectState.Untouched)
                    {
                        GameManager.instance.ChangeScore(-fo.score);
                    }
                    fo.Reclaim();
                }

            }
        }



    }

}
