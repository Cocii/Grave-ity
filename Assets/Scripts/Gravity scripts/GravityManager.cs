using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    [Header("References")]
    public static GravityManager instance;

    [Header("Physics gravity")]
    public Vector2 physicsGravity;
    public Vector2 physicsGravityNormal;
    public Transform physicsGravityTransform;
    public float gravityRatio = 1f;

    [Header("Gravity settings")]
    public Vector2 gravityDirection = Vector2.down;
    public float gravityMagnitude = 9.8f;
    public float transformRotationSpeed = 100f;
    public float valuesLerpSpeed = 100f;
    public bool onlyUpDown = true;
    public float defaultGravityMagnitude = 9.8f;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        physicsGravity = gravityMagnitude * gravityDirection;
    }

    private void Update() {
        if(physicsGravityTransform.up != new Vector3(gravityDirection.x, gravityDirection.y, 0f) || physicsGravity != gravityDirection*gravityMagnitude)
            UpdateGravity();
    }

    private void UpdateGravity() {
        Vector2 targetGravity;

        //RotateTowardsGravityDirection(physicsGravityTransform);

        targetGravity = gravityDirection * gravityMagnitude;
        physicsGravity = Vector2.MoveTowards(physicsGravity, targetGravity, valuesLerpSpeed * Time.deltaTime);

        physicsGravityNormal = -Vector2.Perpendicular(physicsGravity).normalized;

        Physics2D.gravity = physicsGravity;

        gravityRatio = physicsGravity.magnitude / defaultGravityMagnitude;
    }

    

    private void RotateTowardsGravityDirection(Transform toRotate) {
        Vector2 targetPoint = gravityDirection;
        float targetAngle = Mathf.Atan2(targetPoint.x, targetPoint.y) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, -targetAngle));

        if (toRotate.rotation != targetRotation) {
            toRotate.rotation = Quaternion.Lerp(toRotate.rotation, targetRotation, transformRotationSpeed * Time.deltaTime);
        }
        else {
            toRotate.eulerAngles = Utilities.RoundToIntV3(toRotate.eulerAngles);
        }
    }

    public void RotateGravity(Vector2 targetDirection) {
        if ((targetDirection == Vector2.right || targetDirection == Vector2.left) && onlyUpDown)
            return;
        
        gravityDirection = targetDirection;
    }

    public void RotateGravityUpsideDown() {
        Vector2 targetDirection = gravityDirection == Vector2.down ? Vector2.up : Vector2.down;
        gravityDirection = targetDirection;
    }

    public void ModuleGravity(float targetMagnitude) {
        gravityMagnitude = targetMagnitude;
    }

    public void StrongerGravity() {
        if (gravityMagnitude < defaultGravityMagnitude)
            gravityMagnitude = defaultGravityMagnitude;
        else if (gravityMagnitude == defaultGravityMagnitude)
            gravityMagnitude = defaultGravityMagnitude * 1.75f;
    }

    public void WeakerGravity() {
        if (gravityMagnitude > defaultGravityMagnitude)
            gravityMagnitude = defaultGravityMagnitude;
        else if (gravityMagnitude == defaultGravityMagnitude)
            gravityMagnitude = defaultGravityMagnitude * 0.25f;
    }

}
