using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour {

    [Tooltip("TextMeshPro that show the current score")]
    public TextMeshPro tmpro;
    public int initialHP = 20;
    public int Score { get; protected set; }
    public int HealthPoint { get; protected set; }

    public void ChangeScore(int delta)
    {
        Score += delta;
        TextUpdate();
    }

    public void ChangeHealthPoint(int delta)
    {
        HealthPoint += delta;
        if (HealthPoint < 0) HealthPoint = 0;
        TextUpdate();
    }

    private void TextUpdate()
    {
        tmpro.SetText("HP: " + HealthPoint.ToString() + "\n" + "Score: " + Score.ToString());
    }

    private void Start()
    {
        HealthPoint = initialHP;
    }
}
