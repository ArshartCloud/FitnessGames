using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour {

    [Tooltip("TextMeshPro that show the current score")]
    public TextMeshPro tmpro;
    //public int initialHP = 20;
    public int Score { get; protected set; }
    public float totalTime = 300;
    public float timer;
    //public int HealthPoint { get; protected set; }

    public void ChangeScore(int delta)
    {
        Score += delta;
        TextUpdate();
    }

    //public void ChangeHealthPoint(int delta)
    //{
    //    HealthPoint += delta;
    //    if (HealthPoint < 0) HealthPoint = 0;
    //    TextUpdate();
    //}

    public void TextUpdate()
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        tmpro.SetText("Time left: " + niceTime + "\n" + "Score: " + Score.ToString());
    }

    private void Start()
    {
        //HealthPoint = initialHP;
        timer = totalTime;
    }


}
