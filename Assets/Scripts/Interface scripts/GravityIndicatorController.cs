using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GravityIndicatorController : MonoBehaviour
{
    public Vector2 gravity;
    public GravityManager gManager;
    public Slider slider;

    void Start()
    {
        gManager = GravityManager.instance;
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

        slider.value = gravity.magnitude;
    }
}
