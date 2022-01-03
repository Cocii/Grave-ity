using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameMenuManager : MonoBehaviour
{
    [Header("References")]
    public static GameMenuManager instance;
    public AudioOptionSettingScriptable optionSettings;
    private PlayerManager pManager;
    private GameManager gameManager;

    [Header("References")]
    public bool inPause = false;

    [Header("UI References")]
    public GameObject pauseMenuPanel;
    public GameObject gravityInfoPanel;
    public Slider ambiendSoundsVolumeSlider;
    public Slider playerSoundsVolumeSlider;
    public Slider gravityChangeSoundsVolumeSlider;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pManager = PlayerManager.instance;
        gameManager = GameManager.instance;
        InitializeOptions();
    }

    public void SwitchState() {
        gameManager.inPause = !gameManager.inPause;
        inPause = gameManager.inPause;

        //print("Switch: in pause state is " + inPause);

        Cursor.lockState = inPause ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = inPause;

        UpdatePanelsState();

        UpdateInputState();
    }

    private void UpdatePanelsState() {
        if (pauseMenuPanel)
            pauseMenuPanel.SetActive(inPause);

        if (gravityInfoPanel)
            gravityInfoPanel.SetActive(!inPause);
    }

    private void UpdateInputState() {
        if (inPause)
            pManager.input.DisableInput();
        else
            pManager.input.EnableInput();
    }
     
    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void InitializeOptions() {
        ambiendSoundsVolumeSlider.value = optionSettings.ambientSoundsVolume;
        playerSoundsVolumeSlider.value = optionSettings.playerSoundsVolume;
        gravityChangeSoundsVolumeSlider.value = optionSettings.gravityChangeSoundsVolume;
    }

    public void UpdateAmbientVolumeFromSlider() {
        float volume = ambiendSoundsVolumeSlider.value;
        optionSettings.ambientSoundsVolume = volume;
        UpdateAudioSourcesVolumes(gameManager.audioGroup.ambientAudioSources.ToArray(), volume);
    }

    public void UpdatePlayerVolumeFromSlider() {
        float volume = playerSoundsVolumeSlider.value;
        optionSettings.playerSoundsVolume = volume;
        UpdateAudioSourcesVolumes(gameManager.audioGroup.playerAudioSources.ToArray(), volume);
    }

    public void UpdateGravityVolumeFromSlider() {
        float volume = gravityChangeSoundsVolumeSlider.value;
        optionSettings.gravityChangeSoundsVolume = volume;
        UpdateAudioSourcesVolumes(gameManager.audioGroup.gravityChangeAudioSources.ToArray(), volume);
    }

    private void UpdateAudioSourcesVolumes(AudioSource[] sources, float volume) {
        foreach(AudioSource s in sources) {
            if(s != null)
                s.volume = volume;
        }
    }

    private void UpdateAudioSourceVolume(AudioSource source, float volume) {
        source.volume = volume;
    }

}
