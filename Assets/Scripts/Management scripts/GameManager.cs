using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int framerate = 60;
    public bool vSync = false;

    public bool inPause = false;

    public AudioSourceGroup audioGroup;

    public LevelTimer timer;

    //void OnEnable() {
    //    SceneManager.sceneLoaded += OnLevelFinishedLoading;
    //}

    //void OnDisable() {
    //    SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    //}

    //void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
    //    audioGroup.ClearAll();
    //}

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
        if (!vSync) {
            QualitySettings.vSyncCount = 0;
        }
        
    }

    private void OnLevelWasLoaded(int level) {
        
    }

    private void Start()
    {
        timer = GetComponent<LevelTimer>();
        timer.StartTimer();
    }

    public void ManualDestroy()
    {
        if (gameObject != null)
            Destroy(gameObject);
    }

    public void RegisterAmbientAudioSource(AudioSource source) {
        source.volume = audioGroup.audioSettings.ambientSoundsVolume;
        audioGroup.ambientAudioSources.Add(source);
    }

    public void RegisterPlayerAudioSource(AudioSource source) {
        source.volume = audioGroup.audioSettings.playerSoundsVolume;
        audioGroup.playerAudioSources.Add(source);
    }

    public void RegisterEffectsAudioSource(AudioSource source) {
        source.volume = audioGroup.audioSettings.effectsSoundsVolume;
        audioGroup.effectsAudioSources.Add(source);
    }

    public void UnregisterAmbientAudioSource(AudioSource source) {
        if (!audioGroup.ambientAudioSources.Contains(source))
            return;

        audioGroup.ambientAudioSources.Remove(source);
    }

    public void UnregisterPlayerAudioSource(AudioSource source) {
        if (!audioGroup.playerAudioSources.Contains(source))
            return;

        audioGroup.playerAudioSources.Remove(source);
    }

    public void UnregisterEffectsAudioSource(AudioSource source) {
        if (!audioGroup.effectsAudioSources.Contains(source))
            return;

        audioGroup.effectsAudioSources.Remove(source);
    }
}
 
     