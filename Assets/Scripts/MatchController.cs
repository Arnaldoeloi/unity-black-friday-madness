using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchController : MonoBehaviour
{

    public List<PlayerScore> playerScores;

    public float timeRemaining = 120;
    public bool timerIsRunning = false;
    
    public Text timeText;
    public Text matchLastUpdateText;


    // Start is called before the first frame update
    void Start()
    {
        timerIsRunning = true;
        matchLastUpdateText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        updateTime();
    }

    private void updateTime()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);

                if (timeRemaining <= 10 && timeRemaining >= 0)
                {
                    int seconds = Mathf.FloorToInt(timeRemaining % 60);
                    matchLastUpdateText.text = seconds + " seconds remaining!";
                }
            }
            else
            {
                TimeRunOut();
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimeRunOut()
    {
        Debug.Log("Time has run out!");
        Time.timeScale = 0;
        PlayerScore max_score = playerScores[0];
        foreach (PlayerScore score in playerScores)
        {
            if (score.score > max_score.score) max_score = score;
            Debug.Log(score.playerName+": "+score.score);
        }

        timeText.text = "The winner was " + max_score.playerName;

    }
}
