using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    [Header("References")]
    public static GravityManager instance;

    [Header("Physics gravity info")]
    public Vector2 physicsGravity;
    public Vector2 physicsGravityNormal;
    public float gravityRatio = 1f;

    [Header("Gravity settings")]
    public float defaultGravityMagnitude = 9.8f;
    public Vector2 gravityDirection = Vector2.down;
    public float gravityMagnitude = 9.8f;
    public float valuesLerpSpeed = 100f;
    public float weakerGravityMult;
    public float strongergravityMult;
    

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
        UpdateGravity();
    }

    private void UpdateGravity() {
        Vector2 targetGravity = gravityDirection * gravityMagnitude;

        if (physicsGravity == targetGravity)
            return;

        physicsGravity = Vector2.MoveTowards(physicsGravity, targetGravity, valuesLerpSpeed * Time.deltaTime);
        physicsGravityNormal = -Vector2.Perpendicular(physicsGravity).normalized;
        gravityRatio = physicsGravity.magnitude / defaultGravityMagnitude;

        Physics2D.gravity = physicsGravity;
    }

    public void RotateGravity(Vector2 targetDirection) {
        gravityDirection = targetDirection;
    }

    public void RotateGravityUpsideDown() {
        Vector2 targetDirection = gravityDirection == Vector2.down ? Vector2.up : Vector2.down;
        gravityDirection = targetDirection;

        physicsGravity = Vector2.zero;
    }

    public void ModuleGravity(float targetMagnitude) {
        gravityMagnitude = targetMagnitude;
    }

    public void StrongerGravity() {
        if (gravityMagnitude < defaultGravityMagnitude)
            gravityMagnitude = defaultGravityMagnitude;
        else if (gravityMagnitude == defaultGravityMagnitude)
            gravityMagnitude = defaultGravityMagnitude * strongergravityMult;
    }

    public void WeakerGravity() {
        if (gravityMagnitude > defaultGravityMagnitude)
            gravityMagnitude = defaultGravityMagnitude;
        else if (gravityMagnitude == defaultGravityMagnitude)
            gravityMagnitude = defaultGravityMagnitude * weakerGravityMult;
    }

    public void ResetGravity() {
        if (gravityDirection != Vector2.down)
            physicsGravity = Vector2.zero;

        gravityDirection = Vector2.down;
        gravityMagnitude = defaultGravityMagnitude;
    }

}
