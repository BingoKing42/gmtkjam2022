using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float maxTimeValue;

    public float currentTimeValue;
    public Slider timerSlider;
    public TextMeshProUGUI timerText;

    public bool stopTimer = true;

    public void startTimer()
    {
        Debug.Log("timer started");
        timerSlider.maxValue = maxTimeValue;
        timerSlider.value = maxTimeValue;
        currentTimeValue = maxTimeValue;
        stopTimer = false;
    }

    void Update()
    {
        if (!stopTimer)
        {
            if (currentTimeValue > 0)
            {
                currentTimeValue -= Time.deltaTime;
            } else
            {
                currentTimeValue = 0;
                stopTimer = true;
            }

            DisplayTime(currentTimeValue);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        timerSlider.value = timeToDisplay;
    }
}

