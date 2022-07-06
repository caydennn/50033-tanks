using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "GameValues",
        menuName = "GameValues/GameValues",
        order = 0)
]
public class GameValues : ScriptableObject
{
    public int playerLives;
    public int maxPlayerLives;

    public bool gameOver;

    public float enemyRespawnTime;

    public int playerScore;

    public int highScore;



    public int level_threshold_base;
}
