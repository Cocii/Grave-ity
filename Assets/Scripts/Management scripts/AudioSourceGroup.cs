using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceGroup : MonoBehaviour
{
    public List<AudioSource> ambientAudioSources;
    public List<AudioSource> playerAudioSources;
    public List<AudioSource> gravityChangeAudioSources;
    public AudioOptionSettingScriptable audioSettings;

    public void ClearAllNonSingleton() {
        ambientAudioSources.Clear();
    }

    public void ClearAll() {
        ambientAudioSources.Clear();
        playerAudioSources.Clear();
        gravityChangeAudioSources.Clear();
    }
}
