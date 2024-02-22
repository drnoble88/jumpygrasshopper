using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scoring : MonoBehaviour
{
    public int score = 0;
    public int highscore = 0;
    public int finalscore;
    public int finalhighscore;
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI HighScoreText;
    [SerializeField] TextMeshProUGUI FinalScoreText;
    [SerializeField] TextMeshProUGUI FinalHighScoreText;
    void Start()
    {
        score = 0;
    }

    public void AddScore()
    {
        score++;
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void UpdateScore()
    {
        if(score != 1)
        {
            ScoreText.text = "Leaves: " + score;
        }
        else
        {
            ScoreText.text = "Leaf: " + score;
        }
    }

    public void UpdateHighScore()
    {
        HighScoreText.text = "HighScore: " + highscore;
    }
    void Update()
    {
        UpdateScore();
        if (score > highscore)
        {
            highscore = score;
        }
        UpdateHighScore();

        FinalScoreText.text = "Total Leaves: " + score;
        FinalHighScoreText.text = "High Score: " + highscore;
    }
}
