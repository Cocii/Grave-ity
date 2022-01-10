using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnJointBreak : MonoBehaviour
{
    public Joint2D joint;
    public AudioSource source;
    private bool go = true;

    private void Update() {
        if (joint == null && !source.isPlaying && go) {
            source.Play();
            StartCoroutine(LateDestroyCo());
        }
    }

    IEnumerator LateDestroyCo() {
        go = false;

        while (source.isPlaying) {
            //print("Playing");
            yield return null;
        }
        print("coroutine");
        Destroy(this.gameObject);
    }
}
