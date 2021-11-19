using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravitySliderUpdater : MonoBehaviour
{
    public Slider slider;

    private void Start() {
        if (slider == null)
            slider = GetComponent<Slider>();

        slider.maxValue = PlayerManager.instance.resources.maxGravityResourceAmount;
    }

    void Update()
    {
        slider.value = PlayerManager.instance.resources.currentGravityResourceAmount;
    }
}
