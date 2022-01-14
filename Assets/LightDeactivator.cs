using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class LightDeactivator : MonoBehaviour
{
    private Light2D light;

    // Start is called before the first frame update
    void Start()
    {
        light = gameObject.GetComponent<Light2D>();
    }

    private void OnBecameVisible()
    {
        light.enabled = true;
        Debug.Log("VISIBLE");
    }

    private void OnBecameInvisible()
    {
        light.enabled = false;
        Debug.Log("INVISIBLE");
    }
}
