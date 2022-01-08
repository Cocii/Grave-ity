using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class TriggerChecker : MonoBehaviour
{
    public GameObject[] checkpoints;
    private int tempCheckPoint;

    private GameObject currentCheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        currentCheckpoint = checkpoints[0];
        PlayerPrefs.SetInt("currentCheckPoint", 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Checkpoint")
        {
            PlayerManager.instance.input.EnableInput();

            if (!GameObject.ReferenceEquals(currentCheckpoint, collision.gameObject))
            {
                  currentCheckpoint = collision.gameObject;
            }
            

        }

        if (collision.tag == "Death Trigger")
        {
            PlayerManager.instance.input.DisableInput();
            StartCoroutine(DelayReset());
            LevelLoader loader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
            loader.ReloadScene();

        }

        if (collision.tag == "EndLevel")
        {

            //StartCoroutine(DelayReset());
            GameManager manager = GameManager.instance;
            manager.timer.EndTimer();

            float currentTimer = manager.timer.GetTimer();
            string currentTimerStr = currentTimer.ToString();
            PlayerPrefs.SetString("Timer", currentTimerStr);

            LevelLoader loader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
            loader.LoadNextLevel();
            

        }
    }

    public void SaveCheckpoint(Collider2D _col)
    {
        int index = 0;
        int previousIndex = PlayerPrefs.GetInt("currentCheckPoint", 0);
        currentCheckpoint = checkpoints[previousIndex];

        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (GameObject.ReferenceEquals(checkpoints[i], _col.gameObject))
            {
                index = i;
                break;
            }
        }

        if (index > previousIndex)
        {

            tempCheckPoint = previousIndex + 1;
            PlayerPrefs.SetInt("currentCheckPoint", tempCheckPoint);
        }
    }

    IEnumerator DelayReset()
    {
        yield return new WaitForSeconds(0.8f);
        GravityManager.instance.ResetGravity();
        transform.position = currentCheckpoint.transform.position;
    }
}
