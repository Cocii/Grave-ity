using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnTriggerExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player"))
            return;

        PlaySource();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        //print(collision.tag);
        if (collision.CompareTag("Player"))
            return;

        PlaySource();
    }

    private void PlaySource() {
        AudioSource source = GetComponent<AudioSource>();

        if (!source.isPlaying) {
            source.Play();
        }
    }
}
