using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour {
    
    public int score { get; protected set; }
    public TextMeshPro tmpro;

    public void AddScore(int delta)
    {
        score += delta;
        TextUpdate();
    }

    public void TextUpdate()
    {
        tmpro.SetText("Score: " + score.ToString());
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
