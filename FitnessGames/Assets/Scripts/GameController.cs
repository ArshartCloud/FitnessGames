using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public void GamePause()
    {
        Time.timeScale = 0;
    }
    public void GameContinue()
    {
        Time.timeScale = 1;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
