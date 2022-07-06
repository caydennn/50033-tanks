using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public TankManager[] m_Tanks;

    public Mark[] m_Marks;

    public Text m_MessageText;

    public float m_StartDelay = 3f;

    public CameraControl m_CameraControl;

    public GameObject m_PlayerTankPrefab;

    private WaitForSeconds m_StartWait;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Tutorial Mode");
        m_StartWait = new WaitForSeconds(m_StartDelay);

        SpawnPlayerTank();
        SetCameraTargets();
    }

    private void SpawnPlayerTank()
    {
        TankManager playerTank = m_Tanks[0];
        playerTank.m_Instance =
            Instantiate(m_PlayerTankPrefab,
            playerTank.m_SpawnPoint.position,
            playerTank.m_SpawnPoint.rotation) as
            GameObject;
        playerTank.m_PlayerNumber = 1;
        playerTank.SetupPlayerTank(godMode: true);

        StartCoroutine(TutorialLoop());
    }

    private void SetCameraTargets()
    {
        // add the size of marks with the size of m_tanks
        Transform[] targets = new Transform[m_Tanks.Length + m_Marks.Length];

        TankManager playerTank = m_Tanks[0];

        targets[0] = playerTank.m_Instance.transform;
        for (int i = 0; i < m_Marks.Length; i++)
        {
            targets[i + 1] = m_Marks[i].transform;
        }
        Debug.Log("targets length: " + targets.Length);

        // log content of targets
        for (int i = 0; i < targets.Length; i++)
        {
            Debug.Log("targets[" + i + "]: " + targets[i]);
        }

        m_CameraControl.m_Targets = targets;
    }

    private IEnumerator TutorialLoop()
    {
        yield return StartCoroutine(TutorialStarting());
        yield return StartCoroutine(TutorialPlaying());
    }

    private IEnumerator TutorialStarting()
    {
        DisableTankControl();

        m_CameraControl.SetStartPositionAndSize();

        m_MessageText.text = $"TANKS TUTORIAL";

        yield return m_StartWait;
    }

    private IEnumerator TutorialPlaying()
    {
        EnableTankControl();

        m_MessageText.text = string.Empty;

        while ((!TutorialCompleted())) yield return null;
    }

    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++) m_Tanks[i].EnableControl();
    }

    private bool TutorialCompleted()
    {
        // tutorial only completed when player shoots the flashing target
        return true;
    }

    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++) m_Tanks[i].DisableControl();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
