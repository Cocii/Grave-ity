using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;


    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMemoriesPage()
    {
        StartCoroutine(LoadLevel(1));
    }

    public void LoadFirstLevel()
    {
        StartCoroutine(LoadLevel(2));
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void ReloadScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public void BackToMainMenu()
    {
        StartCoroutine(LoadLevel(0));
    }

    public void LoadMainMenuScene()
    {
        PlayerManager.instance.ManualDestroy();
        GravityManager.instance.ManualDestroy();
        Checkpoint.instance.ManualDestroy();
        StartCoroutine(LoadLevel(0));
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }


}
