using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    /*
    //public Slider timerSlider;
    public float gameTime;
    public TextMeshProUGUI timerText;

    public bool stopTimer;

    public void BeginTimer()
    {
        Debug.Log("Timer Started");
        stopTimer = false;
        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime;

        while (!stopTimer)
        {
            float time = gameTime - Time.time;

            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time - minutes * 60f);

            string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);

            if (time <= 0)
            {
                stopTimer = true;
            }

            if (stopTimer == false)
            {
                timerText.text = textTime;
                timerSlider.value = time;
            }
        }
    }
    */
}
