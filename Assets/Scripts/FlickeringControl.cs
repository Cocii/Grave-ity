using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class FlickeringControl : MonoBehaviour
{
    public bool isFlickering = false;
    public float timeDelay;

    public AudioSource flickeringAudio;

    private void Awake() {
        if (flickeringAudio == null)
            flickeringAudio = GetComponentInChildren<AudioSource>();
    }

    void Update()
    {
        if (isFlickering == false)
        {
            StartCoroutine(FlickeringLight());
        }
    }

    IEnumerator FlickeringLight()
    {
        isFlickering = true;
        this.gameObject.GetComponent<Light2D>().enabled = false;
        if (flickeringAudio)
            flickeringAudio.Play();
        timeDelay = Random.Range(0.01f, 0.8f);
        yield return new WaitForSeconds(timeDelay);
        this.gameObject.GetComponent<Light2D>().enabled = true;
        timeDelay = Random.Range(0.01f, 0.8f);
        yield return new WaitForSeconds(timeDelay);
        isFlickering = false;

        
    }
}
