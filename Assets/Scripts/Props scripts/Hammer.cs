using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public float maxHeight;
    public float minHeight;
    public float defaultForceMagnitude;
    public float forceToApply;
    public float weakGravityPushMult;
    public Rigidbody2D body;
    public bool goingUp = false;

    private void Start() {
        if (body == null)
            body = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (goingUp) {
            AddForceUp(defaultForceMagnitude);
            //forceToApply = defaultForceMagnitude;
            CheckLimitPosition();
        }
        else {
            if(transform.localPosition.y <= minHeight) {
                goingUp = true;
            }
            else {
                if (GravityManager.instance.gravityRatio < 1f) {
                    AddForceUp(defaultForceMagnitude * weakGravityPushMult);
                    //forceToApply = defaultForceMagnitude * weakGravityPushMult;
                }
            }

        }
    }

    //private void FixedUpdate() {
    //    if (forceToApply != 0) {
    //        AddForceUp(forceToApply);
    //        forceToApply = 0f;
    //    }
    //}

    private void CheckLimitPosition() {
        if(transform.localPosition.y > maxHeight) {
            transform.localPosition = new Vector3(transform.localPosition.x, maxHeight, transform.localPosition.z);
            body.velocity *= 0f;
            goingUp = false;
        }
    }

    private void AddForceUp(float magnitude) {
        body.AddForce(Vector2.up * magnitude);
    }

}
