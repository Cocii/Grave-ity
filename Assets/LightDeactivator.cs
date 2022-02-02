using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class LightDeactivator : MonoBehaviour
{
    private Light2D lightComponent;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<SpriteRenderer>() == null) {
            gameObject.AddComponent<SpriteRenderer>();
        }

        lightComponent = gameObject.GetComponent<Light2D>();

        if(!GetComponent<SpriteRenderer>().isVisible)
            Activation(false);
    }

    private void OnBecameVisible()
    {
            Activation(true);
    }

    private void OnBecameInvisible()
    {
            Activation(false);
    }

    private void Activation(bool state) {
        if(lightComponent)
            lightComponent.enabled = state;
    }

}
