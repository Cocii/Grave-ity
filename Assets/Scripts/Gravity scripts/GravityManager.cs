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
        print("Updating gravity");
        Vector2 targetGravity;

        
        RotateTowardsGravityDirection(physicsGravityTransform);

        if (onlyUpDown) {
            targetGravity = gravityDirection * gravityMagnitude;
            physicsGravity = Vector2.MoveTowards(physicsGravity, targetGravity, valuesLerpSpeed * Time.deltaTime);
            physicsGravityNormal = RoundToIntV3(physicsGravityTransform.right);
        }
        else {
            targetGravity = physicsGravityTransform.up * gravityMagnitude;
            physicsGravity = RoundV2(Vector2.Lerp(physicsGravity, targetGravity, valuesLerpSpeed * Time.deltaTime));
            physicsGravityNormal = RoundToIntV3(physicsGravityTransform.right);
        }
            
        Physics2D.gravity = physicsGravity;
        
    }

    private Vector2 RoundV2(Vector2 v) {
        v.Set(Mathf.Round(v.x * 10f) / 10f, Mathf.Round(v.y * 10f) / 10f);
        return v;
    }

    private Vector3 RoundV3(Vector3 v) {
        v.Set(Mathf.Round(v.x * 10f) / 10f, Mathf.Round(v.y * 10f) / 10f, Mathf.Round(v.z * 10f) / 10f);
        return v;
    }

    private Vector3 RoundToIntV3(Vector3 v) {
        v.Set(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
        return v;
    }

    private void RotateTowardsGravityDirection(Transform toRotate) {
        Vector2 targetPoint = gravityDirection;
        float targetAngle = Mathf.Atan2(targetPoint.x, targetPoint.y) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, -targetAngle));

        if (toRotate.rotation != targetRotation) {
            toRotate.rotation = Quaternion.Lerp(toRotate.rotation, targetRotation, transformRotationSpeed * Time.deltaTime);
        }
        else {
            toRotate.eulerAngles = RoundToIntV3(toRotate.eulerAngles);
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

    private void OnDrawGizmos() {
        //Gizmos.color = Color.green;
        //Gizmos.DrawRay(transform.position, physicsGravity);


        //Gizmos.color = Color.red;
        //Vector2 targetPoint = new Vector2(transform.position.x, transform.position.y) + gravityDirection;
        //Gizmos.DrawSphere(targetPoint, 0.15f);

        Gizmos.color = Color.white;
    }

}
