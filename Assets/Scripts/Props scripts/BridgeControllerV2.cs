using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeControllerV2 : MonoBehaviour
{
    public SpringJoint2D upHinge;
    public SpringJoint2D downHinge;

    public float frequencyStrong = 5f;
    public float frequencyWeak = 1f;
    public float frequencyOffset = 2.5f;

    public float targetFrequencyUp;
    public float targetFrequencyDown;

    public float defaultTransitionSpeed;
    float transitionSpeed;

    public GravityChangeEventChannelSO gravityChangeChannel;

    public bool stayDown = false;
    public bool wasDown = false;

    GravityManager gManager;

    private void Start() {
        gManager = GravityManager.instance;

        StartCoroutine(LateStart());
    }

    IEnumerator LateStart() {
        yield return new WaitForEndOfFrame();
        UpdateGravity();
        transitionSpeed = defaultTransitionSpeed * 0.334f * Time.smoothDeltaTime;
        //targetFrequencyUp = frequencyStrong;
        //targetFrequencyDown = frequencyWeak;
    }

    private void OnEnable() {
        gravityChangeChannel.OnEventRaised += UpdateGravity;
        gravityChangeChannel.OnEventRaisedVoid += UpdateGravity;
    }

    private void OnDisable() {
        gravityChangeChannel.OnEventRaised -= UpdateGravity;
        gravityChangeChannel.OnEventRaisedVoid -= UpdateGravity;
    }

    public void UpdateGravity(GravityChangesEnum change) {
        UpdateGravity();
    }

    public void UpdateGravity() {
        if (gManager.gravityRatio > 1f && gManager.gravityDirection.y < 0) {
            transitionSpeed = defaultTransitionSpeed * 2f * Time.smoothDeltaTime;
            stayDown = true;
            wasDown = true;
        }
        else {
            stayDown = false;
        }

        UpdateParameters();
    }

    private void UpdateParameters() {
        if (stayDown) {
            targetFrequencyDown = frequencyStrong;
            targetFrequencyUp = frequencyWeak;
        }
        else {
            targetFrequencyUp = Random.Range(frequencyStrong - frequencyOffset, frequencyStrong + frequencyOffset);
            targetFrequencyDown = frequencyWeak;
        }
    }

    private void FixedUpdate() {
        if (FrequenciesTransitioned() && stayDown) {
            return;
        }


        upHinge.frequency = Mathf.MoveTowards(upHinge.frequency, targetFrequencyUp, transitionSpeed);
        downHinge.frequency = Mathf.MoveTowards(downHinge.frequency, targetFrequencyDown, transitionSpeed);

        if (FrequenciesTransitioned() && !stayDown) {
            if (wasDown) {
                wasDown = false;
                transitionSpeed = defaultTransitionSpeed * 0.334f * Time.smoothDeltaTime;
            }

            UpdateParameters();
        }


        //print("moving bridge");
    }

    private bool FrequenciesTransitioned() {
        return upHinge.frequency == targetFrequencyUp && downHinge.frequency == targetFrequencyDown;
    }
}
