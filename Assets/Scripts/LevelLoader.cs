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
        StartCoroutine(LoadLevel(5));
    }

    public void LoadTimerPage()
    {
        DestroySingletons();

        StartCoroutine(LoadLevel(2));
    }

    public void LoadFirstLevel()
    {
        StartCoroutine(LoadLevel(1));
    }
    public void LoadSecondLevel()
    {
        StartCoroutine(LoadLevel(3));
    }

    public void LoadNextLevel()
    {

        DestroySingletons();
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
        DestroySingletons();
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

    private void DestroySingletons() {
        if(GameManager.instance)
            GameManager.instance.ManualDestroy();
        
        if(PlayerManager.instance)
            PlayerManager.instance.ManualDestroy();
        
        if(GravityManager.instance)
            GravityManager.instance.ManualDestroy();
        
        if(Checkpoint.instance)
            Checkpoint.instance.ManualDestroy();
    }
}
