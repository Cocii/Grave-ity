using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GUITimer : MonoBehaviour
{
    string timerString;
    float timer;

    string minutes, seconds;

    [SerializeField]
    GameObject timerText;



    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        timerString = PlayerPrefs.GetString("Timer");

        if(timerString == null)
        {
            timerString = "0.0";
        }

        timer = float.Parse(timerString);


        minutes = Mathf.Floor(timer / 60).ToString("00");
        seconds = (timer % 60).ToString("00");

        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        timerText.GetComponent<TMPro.TextMeshProUGUI>().text = niceTime;

    }

    
}
