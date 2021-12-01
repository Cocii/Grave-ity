using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private static Checkpoint instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ResetCheckpoints();
    }

    public void ResetCheckpoints()
    {
        PlayerPrefs.SetInt("currentCheckPoint", 0);
        Debug.Log(PlayerPrefs.GetInt("currentCheckPoint", 0));
    }

    //CALL THIS IF YOU FINISH THE LEVEL
    public void ManuallyDestroy()
    {
        if(instance)
        {
            Destroy(this.gameObject);
        }
    }
}
