using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGo : StateMachineBehaviour
{
    //public GameObject go1;
    //public GameObject go2;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        GameObject world = GameObject.Find("World");
        world = world.transform.Find("SquatAnimation").gameObject;
        GameObject go1 = world.transform.Find("CA1").gameObject;
        GameObject go2 = world.transform.Find("CA2").gameObject;
        if (go1.activeInHierarchy)
        {
            go1.SetActive(false);
            go2.SetActive(true);
        }
        else
        {
            go1.SetActive(true);
            go2.SetActive(false);
        }
    }

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
