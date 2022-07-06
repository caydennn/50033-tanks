using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mark : MonoBehaviour
{
    public bool isFinisher;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Shell")
        {
            Debug.Log("Shell hit the mark");
            if (isFinisher)
            {
                // Load the next scene
                SceneManager
                    .LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
