﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;

    [Tooltip("Score System to calculate score")]
    public ScoreSystem scoreSystem;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            print("\n\nWarning! Multiple GameManager!\n\n");
    }

        public void GamePause()
    {
        Time.timeScale = 0;
    }
    public void GameContinue()
    {
        Time.timeScale = 1;
    }

    public void ChangeScore(int delta)
    {
        scoreSystem.ChangeScore(delta);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}