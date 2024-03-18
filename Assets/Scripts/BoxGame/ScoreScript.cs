using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreScript : MonoBehaviour
{
    public int playerScore;
    public TextMesh scoreText;

    public void addScore()
    {
        playerScore += 1;
        scoreText.text = playerScore.ToString();
    }

    public int finalScore()
    {
        return playerScore;
    }

    public void resetScore()
    {
        playerScore = 0;
        scoreText.text = playerScore.ToString();
    }
}
