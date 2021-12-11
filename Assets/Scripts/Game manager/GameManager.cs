using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int framerate = 60;

    public AudioSource[] ambientAudioSources;
    public AudioSource[] playerAudioSources;
    public AudioSource[] gravityChangeAudioSources;

    public bool inPause = false;

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
}
