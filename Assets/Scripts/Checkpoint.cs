using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Checkpoint instance;

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

    public void ManualDestroy()
    {
        if(gameObject != null)
            Destroy(gameObject);
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

}
