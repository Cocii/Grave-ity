using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    float timer;
    bool updateTimer = false;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.00f;
    }

    // Update is called once per frame
    void Update()
    {
        if(updateTimer)
        {
            timer += Time.deltaTime;
        }
    }

    public float GetTimer()
    {
        return timer;
    }

    public void StartTimer()
    {
        PlayerPrefs.SetString("Timer", "0.0");
        updateTimer = true;
    }

    public void EndTimer()
    {
        updateTimer = false;
    }
}
