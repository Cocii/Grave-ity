using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRegisterAudioSource : MonoBehaviour
{
    public enum SourceTypeEnum {
        Player,
        Ambient,
        GravityChange
    }

    public SourceTypeEnum sourceType;

    private void Start() {
        AudioSource source = GetComponent<AudioSource>();
        switch (sourceType) {
            case SourceTypeEnum.Player:
                GameManager.instance.RegisterPlayerAudioSource(source);
                break;

            case SourceTypeEnum.Ambient:
                GameManager.instance.RegisterAmbientAudioSource(source);
                break;

            case SourceTypeEnum.GravityChange:
                GameManager.instance.RegisterGravityChangeAudioSource(source);
                break;

            default:
                break;

        }
    }
}
