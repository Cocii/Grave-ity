using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnTrigger : MonoBehaviour
{
    public enum triggerModeEnum {
        enter, 
        exit,
        both
    }

    public triggerModeEnum mode;
    public LayerMask layerMask;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (mode == triggerModeEnum.enter || mode == triggerModeEnum.both) {
            //if ((layer.value & (1 << collision.transform.gameObject.layer)) > 0) {
            //    PlaySource();
            //    //print(collision.tag);
            //}
            if (Utilities.LayerInLayermask(collision.gameObject.layer, layerMask)) {
                PlaySource();
                //print(collision.tag);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (mode == triggerModeEnum.exit || mode == triggerModeEnum.both) {
            //if ((layerMask.value & (1 << collision.transform.gameObject.layer)) > 0) {
            //    PlaySource();
            //    //print(collision.tag);
            //}
            if (Utilities.LayerInLayermask(collision.gameObject.layer, layerMask)) {
                PlaySource();
                //print(collision.tag);
            }
        }
    }

    private void PlaySource() {
        AudioSource source = GetComponent<AudioSource>();

        if (!source.isPlaying) {
            source.Play();
        }
    }
}
