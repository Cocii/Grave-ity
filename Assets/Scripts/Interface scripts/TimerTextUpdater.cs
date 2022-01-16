using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerTextUpdater : MonoBehaviour
{
    public Text timerText;
    public LevelTimer timer;

    public float delay = 1f;
    public float timeSinceLastUpdate = 0f;

    void Start()
    {
        if (timerText == null)
            timerText = GetComponent<Text>();
        timerText.text = "00:00";

        timer = GameManager.instance.timer;
    }

    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate > delay) {
            timeSinceLastUpdate = 0f;
            UpdateTimer();
        }
    }

    private void UpdateTimer() {
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
