using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLights : MonoBehaviour
{
    public bool switch_lights = true;

    // Start is called before the first frame update
    void Start()
    {
        switch_lights = PlayerPrefs.GetInt("Lights") == 1 ? true : false;

        gameObject.SetActive(switch_lights);
        
    }

    public void On()
    {
        PlayerPrefs.SetInt("Lights", 1);
        switch_lights = PlayerPrefs.GetInt("Lights") == 1 ? true : false;
        gameObject.SetActive(switch_lights);
    }

    public void Off()
    {
        PlayerPrefs.SetInt("Lights", 0);
        switch_lights = PlayerPrefs.GetInt("Lights") == 1 ? true : false;
        gameObject.SetActive(switch_lights);
    }
}
