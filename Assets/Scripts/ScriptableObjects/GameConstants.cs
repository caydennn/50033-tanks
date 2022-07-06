using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "GameConstants",
        menuName = "Constants/GameConstants",
        order = 0)
]
public class GameConstants : ScriptableObject
{
    public int playerLives;

    public bool gameOver;

    public float enemyRespawnTime;

    public int playerScore;

    public int highScore;



    public int level_threshold_base;
}
