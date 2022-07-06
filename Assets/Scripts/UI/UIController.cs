using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Text scoreText;

    public Text playerLivesText;

    public GameValues gameValues;
    private int levelThreshold;

    // Start is called before the first frame update
    void Start()
    {
        levelThreshold =
            SceneManager.GetActiveScene().buildIndex *
            gameValues.level_threshold_base;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + gameValues.playerScore.ToString() + "/" + levelThreshold.ToString();
        playerLivesText.text =
            "Lives Left: ";
        for (int i = 0; i < gameValues.playerLives; i++)
        {
            playerLivesText.text += "❤️ ";
        }
        // playerLivesText.text =
        //     "Lives Left: " + gameValues.playerLives * "❤️";
    }
}
