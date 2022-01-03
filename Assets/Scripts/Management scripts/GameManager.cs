using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int framerate = 60;

    public bool inPause = false;

    public AudioSourceGroup audioGroup;

    void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        audioGroup.ClearAll();
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = framerate;
        
    }

    private void OnLevelWasLoaded(int level) {
        
    }

    private void Update() {
        if(Application.targetFrameRate != framerate)
            Application.targetFrameRate = framerate;
    }

    public void RegisterAmbientAudioSource(AudioSource source) {
        if (audioGroup.ambientAudioSources.Contains(source))
            return;

        source.volume = audioGroup.audioSettings.ambientSoundsVolume;
        audioGroup.ambientAudioSources.Add(source);
    }

    public void RegisterPlayerAudioSource(AudioSource source) {
        source.volume = audioGroup.audioSettings.playerSoundsVolume;
        audioGroup.playerAudioSources.Add(source);
    }

    public void RegisterGravityChangeAudioSource(AudioSource source) {
        source.volume = audioGroup.audioSettings.gravityChangeSoundsVolume;
        audioGroup.gravityChangeAudioSources.Add(source);
    }
}
 
     