using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerTextUpdater : MonoBehaviour
{
    public Text timerText;
    public LevelTimer timer;
    
    void Start()
    {
        if (timerText == null)
            timerText = GetComponent<Text>();
        timerText.text = "00:00";

        timer = GameManager.instance.timer;
    }

    void Update()
    {
        if (timer) {
            float time = timer.GetTimer();
            string timeString = Utilities.FormatTimeMinSec(time);
            timerText.text = timeString;
        }
        else {
            timer = GameManager.instance.timer;
        }  
    }

    
}
