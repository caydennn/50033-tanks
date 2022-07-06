using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public Gradient gradient;

    public Image fill;

    public AudioSource lowHealthSound;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        Debug.Log(fill.color);
        Debug.Log(slider.normalizedValue);
        if (fill.color == Color.red && !lowHealthSound.isPlaying && health >= 0)
        {
            lowHealthSound.Play();
        }
    }
}
