using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text scoreText;
    public Text playerLivesText;



    public GameConstants gameConstants;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // TimeSpan timeSpan = TimeSpan.FromSeconds(gameState.gameTime);
        // gameTimeText.text = timeSpan.ToString("m':'ss");
        scoreText.text = "Score: " + gameConstants.playerScore.ToString();
        playerLivesText.text = "Lives Left: " + gameConstants.playerLives.ToString();
       
    }
}
