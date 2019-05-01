using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Change(GameObject other)
    {
        other.SetActive(true);
        gameObject.SetActive(false);
    }
}
