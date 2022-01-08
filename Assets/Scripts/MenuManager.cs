using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public LevelLoader levelLoader;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        levelLoader.LoadNextLevel();
    }

    public void ResetSavegame()
    {
        PlayerPrefs.SetInt("currentCheckPoint", 0);
    }

    public void Back()
    {
        levelLoader.BackToMainMenu();
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
