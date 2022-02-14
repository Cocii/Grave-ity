using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public float UpAngleDegree;
    public float DownAngleDegree;

    public float targetAngleDegree = 0f;
    Quaternion targetAngle;
    public float defaultRotationSpeed = 10f;
    float rotationSpeed = 10f;
    public bool rotating = false;
    public GravityChangeEventChannelSO gravityChangeChannel;
    GravityManager gManager;

    private void Start() {
        gManager = GravityManager.instance;

        UpdateGravity();
    }

    private void OnEnable() {
        gravityChangeChannel.OnEventRaised += UpdateGravity;
        gravityChangeChannel.OnEventRaisedVoid += UpdateGravity;
    }

    private void OnDisable() {
        gravityChangeChannel.OnEventRaised -= UpdateGravity;
        gravityChangeChannel.OnEventRaisedVoid -= UpdateGravity;
    }

    private void UpdateAngle() {
        targetAngle = Quaternion.Euler(0f, 0f, targetAngleDegree);
    }

    IEnumerator RotationCo() {
        rotating = true;
        print("Rotating bridge co started");

        var wait = new WaitForEndOfFrame();
        while (transform.rotation != targetAngle) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetAngle, rotationSpeed);
            //print(transform.eulerAngles.z +" is equal to "+ targetAngleDegree + "? " + (transform.eulerAngles.z == targetAngleDegree));
            //print(transform.rotation + " is equal to " + targetAngle + "? " + (transform.rotation == targetAngle));
            yield return wait; 
        }

        print("Rotating bridge co ended");
        rotating = false;
        yield break;
    }

    public void UpdateGravity(GravityChangesEnum change) {
        UpdateGravity();
    }

    public void UpdateGravity() {
        if (gManager.gravityRatio > 1f && gManager.gravityDirection.y < 0) {
            targetAngleDegree = DownAngleDegree;
            rotationSpeed = defaultRotationSpeed;
        }
        else {
            targetAngleDegree = UpAngleDegree;
            rotationSpeed = defaultRotationSpeed * 0.334f;
        }

        UpdateAngle();

        if (!rotating) {
            StartCoroutine(RotationCo());
        }
    }
    
}
