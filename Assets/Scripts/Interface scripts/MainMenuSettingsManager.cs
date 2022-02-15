using UnityEngine;
using UnityEngine.UI;

public class MainMenuSettingsManager : MonoBehaviour {
    public AudioOptionSettingScriptable audioSettings;
    public FloatEventChannelSO musicVolumeChangeChannel;
    [Space]
    public Slider ambientVolumeSlider;
    public Slider playerVolumeSlider;
    public Slider effectsVolumeSlider;
    public Slider musicVolumeSlider;

    private void Start() {
        InitializeSettings();
    }

    private void InitializeSettings() {
        ambientVolumeSlider.value = audioSettings.ambientSoundsVolume;
        playerVolumeSlider.value = audioSettings.playerSoundsVolume;
        effectsVolumeSlider.value = audioSettings.effectsSoundsVolume;
        musicVolumeSlider.value = audioSettings.musicVolume;
    }

    public void UpdateAmbientVolume() {
        audioSettings.ambientSoundsVolume = ambientVolumeSlider.value;
    }

    public void UpdatePlayerVolume() {
        audioSettings.playerSoundsVolume = playerVolumeSlider.value;
    }

    public void UpdateEffectsVolume() {
        audioSettings.effectsSoundsVolume = effectsVolumeSlider.value;
    }

    public void UpdateMusicVolume() {
        audioSettings.musicVolume = musicVolumeSlider.value;
        musicVolumeChangeChannel.RaiseEvent(audioSettings.musicVolume);
    }

}
