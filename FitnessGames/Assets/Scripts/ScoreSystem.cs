using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour {

    [Tooltip("TextMeshPro that show the current score")]
    public TextMeshPro tmpro;

    public int score { get; protected set; }

    public void AddScore(int delta)
    {
        score += delta;
        TextUpdate();
    }

    public void TextUpdate()
    {
        tmpro.SetText("Score: " + score.ToString());
    }
}
