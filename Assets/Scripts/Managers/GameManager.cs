using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;

    public float m_StartDelay = 3f;

    public float m_EndDelay = 3f;

    public CameraControl m_CameraControl;

    public Text m_MessageText;

    public GameObject[] m_TankPrefabs;

    public TankManager[] m_Tanks;

    public List<Transform> wayPointsForAI;

    public GameValues gameValues;

    private int m_RoundNumber;

    private WaitForSeconds m_StartWait;

    private WaitForSeconds m_EndWait;

    private TankManager m_RoundWinner;

    private TankManager m_GameWinner;

    private int levelThreshold;

    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        // scale the threshold based on the current level
        levelThreshold = SceneManager.GetActiveScene().buildIndex * gameValues.level_threshold_base;

        SpawnAllTanks();
        SetCameraTargets();

        StartCoroutine(GameLoop());
    }

    private void SpawnAllTanks()
    {
        // the player
        m_Tanks[0].m_Instance =
            Instantiate(m_TankPrefabs[0],
            m_Tanks[0].m_SpawnPoint.position,
            m_Tanks[0].m_SpawnPoint.rotation) as
            GameObject;
        m_Tanks[0].m_PlayerNumber = 1;
        m_Tanks[0].SetupPlayerTank(godMode: false);

        // the AI
        for (int i = 1; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].m_Instance =
                Instantiate(m_TankPrefabs[i],
                m_Tanks[i].m_SpawnPoint.position,
                m_Tanks[i].m_SpawnPoint.rotation) as
                GameObject;
            m_Tanks[i].m_PlayerNumber = i + 1;
            m_Tanks[i].SetupAI(wayPointsForAI);
        }
    }

    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[m_Tanks.Length];

        for (int i = 0; i < targets.Length; i++)
        targets[i] = m_Tanks[i].m_Instance.transform;

        m_CameraControl.m_Targets = targets;
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (
            (
            gameValues.playerScore >= levelThreshold &&
            SceneManager.GetActiveScene().buildIndex <
            SceneManager.sceneCountInBuildSettings
            )
        )
        {
            SceneManager
                .LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
            StartCoroutine(GameLoop()); // restarts the loop...
    }

    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();
        ResetConstants();

        m_CameraControl.SetStartPositionAndSize();

        m_RoundNumber++;
        StringBuilder sb = new StringBuilder();
        sb.Append($"{SceneManager.GetActiveScene().name}\n");
        sb.Append("\n");
        sb.Append($" Round {m_RoundNumber}");
        m_MessageText.text = sb.ToString();

        yield return m_StartWait;
    }

    private IEnumerator RoundPlaying()
    {
        EnableTankControl();

        m_MessageText.text = string.Empty;

        while (!gameValues.gameOver) yield return null;
        // while (!OneTankLeft()) yield return null;
    }

    private IEnumerator RoundEnding()
    {
        DisableTankControl();

        m_MessageText.text = MyEndMessage();
        yield return m_EndWait;

        // m_RoundWinner = null;

        // m_RoundWinner = GetRoundWinner();
        // if (m_RoundWinner != null) m_RoundWinner.m_Wins++;

        // m_GameWinner = GetGameWinner();

        // string message = EndMessage();
        // m_MessageText.text = message;

        // yield return m_EndWait;
    }

    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf) numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }

    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf) return m_Tanks[i];
        }

        return null;
    }

    private TankManager GetGameWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Wins == m_NumRoundsToWin) return m_Tanks[i];
        }

        return null;
    }

    private string MyEndMessage()
    {
        StringBuilder sb = new StringBuilder();
        if (gameValues.playerScore < levelThreshold)
        {
            sb.Append("Game Over!\n");
            sb.Append("\n");
            sb.Append("You scored: " + gameValues.playerScore + "\n");
            sb.Append("\n");

            sb.Append("Your High Score: " + gameValues.highScore + "\n");
            sb.Append("\n");
            sb
                .Append("You need at least: " +
                levelThreshold +
                " to proceed!" +
                "\n");
        }
        else
        {
            sb.Append("Nice job! You made it to the next level!\n");
            sb.Append("\n");
            sb.Append("You scored: " + gameValues.playerScore + "\n");
            sb.Append("\n");
            sb.Append("Your High Score: " + gameValues.highScore + "\n");
            sb.Append("\n");
        }

        return sb.ToString();
    }

    // private string EndMessage()
    // {
    //     var sb = new StringBuilder();
    //     if (m_RoundWinner != null)
    //         sb.Append($"{m_RoundWinner.m_ColoredPlayerText} WINS THE ROUND!");
    //     else
    //         sb.Append("DRAW!");
    //     sb.Append("\n\n\n\n");
    //     for (int i = 0; i < m_Tanks.Length; i++)
    //     {
    //         sb
    //             .AppendLine($"{m_Tanks[i].m_ColoredPlayerText}: {
    //                 m_Tanks[i].m_Wins} WINS");
    //     }
    //     if (m_GameWinner != null)
    //         sb.Append($"{m_GameWinner.m_ColoredPlayerText} WINS THE GAME!");
    //     return sb.ToString();
    // }
    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++) m_Tanks[i].Reset();
    }

    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++) m_Tanks[i].EnableControl();
    }

    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++) m_Tanks[i].DisableControl();
    }

    private void ResetConstants()
    {
        gameValues.playerLives = 1;
        gameValues.playerScore = 0;
        gameValues.gameOver = false;
    }
}
