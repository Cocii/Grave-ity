using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GravityIndicatorController : MonoBehaviour
{
    public Vector2 gravity;
    public GravityManager gManager;
    public Slider slider;
    public RectTransform handle;

    void Start()
    {
        gManager = GravityManager.instance;

        slider.maxValue = gManager.defaultGravityMagnitude * gManager.strongerGravityMult;
    }


    void Update()
    {
        Vector2 currentGravity = gManager.physicsGravity;
        if(currentGravity != gravity) {
            gravity = currentGravity;
            UpdateIndicator();
        }
    }

    private void UpdateIndicator() {
        float targetAngle = 0f;
        if (gravity.normalized == Vector2.up)
            targetAngle = 180f;

        transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
        handle.rotation = Quaternion.Euler(0f, 0f, targetAngle + 180f);

        slider.value = gravity.magnitude;
    }
}
