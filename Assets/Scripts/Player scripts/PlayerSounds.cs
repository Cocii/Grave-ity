using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public PlayerManager manager;

    [Header("Steps")]
    public AudioSource stepAudioSource;
    public AudioClip[] grassStepAudioClips;
    public AudioClip[] glassStepAudioClips;
    public AudioClip[] metalStepAudioClips;

    [Header("Gravity changes")]
    public AudioSource gravityChangeAudioSource;
    public AudioClip gravityWeakerClip;
    public AudioClip gravityStrongerClip;

    private void Start() {
        manager = PlayerManager.instance;
    }

    public void PlayStepSound() {
        if (!stepAudioSource)
            return;

        //print("Playing step from " + gameObject.transform.parent.name);

        switch (manager.groundType) {
            case GroundTypeEnum.grass:
                stepAudioSource.clip = SelectRandomFrom(grassStepAudioClips);
                stepAudioSource.Play();
                break;

            case GroundTypeEnum.glass:
                stepAudioSource.clip = SelectRandomFrom(glassStepAudioClips);
                stepAudioSource.Play();
                break;

            case GroundTypeEnum.metal:
                stepAudioSource.clip = SelectRandomFrom(metalStepAudioClips);
                stepAudioSource.Play();
                break;

            default:
                print("Ground type not implemented");
                break;
        }
    }

    private AudioClip SelectRandomFrom(AudioClip[] possibilities) {
        return possibilities[Random.RandomRange(0, possibilities.Length)];
    }

    public void PlayGravvityWeaker() {
        gravityChangeAudioSource.clip = gravityWeakerClip;
        gravityChangeAudioSource.Play();
    }

    public void PlayGravvityStronger() {
        gravityChangeAudioSource.clip = gravityStrongerClip;
        gravityChangeAudioSource.Play();
    }

    public void PlayGravityChange(GravityChangesEnum changeRequested) {
        switch (changeRequested) {
            case GravityChangesEnum.Weaker:
                //print("player sounds with request: " + changeRequested);
                PlayGravvityWeaker();
                break;

            case GravityChangesEnum.Stronger:
                //print("player sounds with request: " + changeRequested);
                PlayGravvityStronger();
                break;

            default:
                //print("sound default case");
                break;
        }
    }
}
