using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public float upAngleDegree;
    public Vector2 upAngleDegreeRange;
    public float downAngleDegree;
    
    public float targetAngleDegree = 0f;
    Quaternion targetAngle;
    public float defaultRotationSpeed = 10f;
    float rotationSpeed = 10f;
    public bool rotating = false;
    public GravityChangeEventChannelSO gravityChangeChannel;
    GravityManager gManager;

    public bool stayDown = false;
    public bool wasDown = false;

    private void Start() {
        gManager = GravityManager.instance;

        StartCoroutine(LateStart());
    }

    IEnumerator LateStart() {
        yield return new WaitForEndOfFrame();
        UpdateGravity();
        rotationSpeed = defaultRotationSpeed * 0.334f * Time.smoothDeltaTime;
    }

    private void OnEnable() {
        gravityChangeChannel.OnEventRaised += UpdateGravity;
        gravityChangeChannel.OnEventRaisedVoid += UpdateGravity;
    }

    private void OnDisable() {
        gravityChangeChannel.OnEventRaised -= UpdateGravity;
        gravityChangeChannel.OnEventRaisedVoid -= UpdateGravity;
    }

    private void UpdateTargetAngle() {
        if (stayDown) {
            targetAngleDegree = downAngleDegree;
        }
        else {
            targetAngleDegree = Random.Range(upAngleDegreeRange.x, upAngleDegreeRange.y);
        }

        targetAngle = Quaternion.Euler(0f, 0f, targetAngleDegree);
    }

    private void FixedUpdate() {
        if (transform.rotation == targetAngle && stayDown) {
            return;
        }
        


        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetAngle, rotationSpeed);

        if (transform.rotation == targetAngle && !stayDown) {
            if (wasDown) {
                wasDown = false;
                rotationSpeed = defaultRotationSpeed * 0.334f * Time.smoothDeltaTime;
            }

            UpdateTargetAngle();
        }
            

        //print("moving bridge");
    }

    //IEnumerator RotationCo() {
    //    rotating = true;
    //    print("Rotating bridge co started");

    //    var wait = new WaitForEndOfFrame();
    //    while (transform.rotation != targetAngle) {
    //        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetAngle, rotationSpeed);

    //        yield return wait; 
    //    }

    //    print("Rotating bridge co ended");
    //    rotating = false;
    //    yield break;
    //}

    public void UpdateGravity(GravityChangesEnum change) {
        UpdateGravity();
    }

    public void UpdateGravity() {
        if (gManager.gravityRatio > 1f && gManager.gravityDirection.y < 0) {
            rotationSpeed = defaultRotationSpeed * 2f * Time.smoothDeltaTime;
            stayDown = true;
            wasDown = true;
        }
        else {
            //rotationSpeed = defaultRotationSpeed * 0.334f * Time.smoothDeltaTime;
            stayDown = false;
        }

        UpdateTargetAngle();

        //if (!rotating) {
        //    StartCoroutine(RotationCo());
        //}
    }

}
