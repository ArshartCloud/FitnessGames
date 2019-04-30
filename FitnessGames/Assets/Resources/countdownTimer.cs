using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    float currentTime = 0f;
    float startingTime = 10f; 

	// Use this for initialization
	void Start () {
        currentTime = startingTime;

	}
	
	// Update is called once per frame
	void Update () {
        currentTime -= 1;
	}
}
