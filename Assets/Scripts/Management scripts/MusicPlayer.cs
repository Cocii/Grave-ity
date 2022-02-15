using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;
    public AudioSource musicSource;
    public FloatEventChannelSO musicVolumeChangeChannel;
    public AudioOptionSettingScriptable audioSettings;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        UpdateVolume(audioSettings.musicVolume);
    }

    private void OnEnable() {
        musicVolumeChangeChannel.OnEventRaised += UpdateVolume;
    }

    private void OnDisable() {
        musicVolumeChangeChannel.OnEventRaised -= UpdateVolume;
    }

    public void ManualDestroy() {
        if (gameObject != null)
            Destroy(gameObject);
    }

    public void UpdateVolume(float volume) {
        musicSource.volume = volume;
    }
}
