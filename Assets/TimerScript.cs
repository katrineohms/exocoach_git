using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    [Header("Timer")]
    public GameObject TimerObject;
    public TextMesh TimerText;
    public int totalTime = 3 * 60;
    private float timer;

    [Header("End UI")]
    public GameObject endScreen;
    public ScoreScript scoreManager;
    public TextMeshProUGUI finalScoreText;

    [Header("Resetting")]
    public GameObject cursor;
    public GameObject hitBoxSpawner;

    // Start is called before the first frame update
    void Start()
    {
        timer = totalTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer = timer - Time.deltaTime;
            UpdateTimerText();
        }

        else
        {
            Debug.Log("Time's up!");
            endScreen.SetActive(true);
            TimerObject.SetActive(false);
            int finalScore = scoreManager.finalScore();
            finalScoreText.text = "You scored:  " + finalScore.ToString();

            //Resetting
            scoreManager.resetScore();
            hitBoxSpawner.SetActive(false);
            Destroy(GameObject.FindGameObjectWithTag("HitBox"));
        }
            
        
    }
    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetTimer()
    {
        timer = totalTime;
    }
}
