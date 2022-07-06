using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourChange : MonoBehaviour
{
    private Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        StartCoroutine(ChangeColor());
    }

    private IEnumerator ChangeColor()
    {
        while (true)
        {
            renderer.material.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            renderer.material.color = Color.blue;
            yield return new WaitForSeconds(0.5f);
            renderer.material.color = Color.green;
            yield return new WaitForSeconds(0.5f);
            renderer.material.color = Color.yellow;
            yield return new WaitForSeconds(0.5f);
            renderer.material.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
    }

  
}
