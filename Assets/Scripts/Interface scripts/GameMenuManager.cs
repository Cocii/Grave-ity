using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    [Header("References")]
    public static GameMenuManager instance;
    private PlayerManager pManager;
    private GameManager gameManager;
    private LevelLoader loader;
    
    [Header("Options")]
    public AudioOptionSettingScriptable optionSettings;
    public PerformanceOptionSettingScriptable performanceOptions;

    [Header("Channels")]
    public VoidEventChannelSO switchStateChannel;

    [Header("Bools")]
    public bool inPause = false;

    [Header("Panels")]
    public GameObject pauseMenuPanel;
    public List<GameObject> panelsToDeactivate;

    [Header("Sliders")]
    public Slider ambiendSoundsVolumeSlider;
    public Slider playerSoundsVolumeSlider;
    public Slider gravityChangeSoundsVolumeSlider;

    [Header("Toggles")]
    public Toggle lowPerformanceToggle;
    public Toggle showComandsToggle;

    [Header("Texts")]
    public Text levelNameText;
    public Text timerText;

    [Header("Scrollbar")]
    public Scrollbar menuScrollbar;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        //DontDestroyOnLoad(gameObject);
    }

    void Start() {
        loader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
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

        switchStateChannel.RaiseEvent();
    }

    private void UpdatePanelsState() {
        menuScrollbar.value = 1;

        if (pauseMenuPanel) {
            pauseMenuPanel.SetActive(inPause);
        }

        foreach(GameObject pnl in panelsToDeactivate) {
            if (pnl)
                pnl.SetActive(!inPause);
        }

        
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

    public void BackToMainMenu()
    {
        loader.LoadMainMenuScene();
    }

    public void InitializeOptions() {
        ambiendSoundsVolumeSlider.value = optionSettings.ambientSoundsVolume;
        playerSoundsVolumeSlider.value = optionSettings.playerSoundsVolume;
        gravityChangeSoundsVolumeSlider.value = optionSettings.effectsSoundsVolume;

        string name="";
        switch (SceneManager.GetActiveScene().buildIndex) {
            case 1:
                name = "Forest";
                break;
            case 3:
                name = "Lab";
                break;
            default:
                name = "Level not in build";
                break;
        }
        levelNameText.text = name;

        //timerText.text = "00:00";
        LowPerformanceToggleActivation();
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
        optionSettings.effectsSoundsVolume = volume;
        UpdateAudioSourcesVolumes(gameManager.audioGroup.effectsAudioSources.ToArray(), volume);
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

    public void LowPerformanceToggleActivation() {
        bool activation = lowPerformanceToggle.isOn;

        //print("Low performance is: " + activation);
    }

    public void ComandsActivation() {
        UIManager.instance.ActivateComandsPanel(showComandsToggle.isOn);
    }
}
