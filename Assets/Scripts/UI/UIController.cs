using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text scoreText;

    public Text playerLivesText;

    public GameValues gameValues;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + gameValues.playerScore.ToString();
        playerLivesText.text =
            "Lives Left: " + gameValues.playerLives.ToString();
    }
}
