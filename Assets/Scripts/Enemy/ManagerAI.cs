using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum InitialPoseEnum {
    standard,
    crouched
}

public class ManagerAI : MonoBehaviour
{
    public InitialPoseEnum initialPose;

    [Header("References")]
    public static PlayerManager instance;
    public Rigidbody2D body;
    public CapsuleCollider2D bodyCollider;
    public Animator animator;
    public CharacterControllerAI characterController;
    public MovementAI movement;
    public GameObject headBone;
    GravityManager gManager;
    public List<GameObject> childrens;
    public EventsAI events;


    [Header("Gravity info")]
    public Vector2 currentGravity;
    public Vector2 currentGravityNormal;
    public float currentGravityRatio;

    [Header("Ground info")]
    public GroundTypeEnum groundType;

    [Header("Collider info")]
    public Vector2 defaultColliderSize;
    public Vector2 defaultColliderOffset;
    public Vector2 scaledColliderSize;
    public Vector2 scaledCrouchedColliderSize;

    [Header("Ground check settings")]
    public LayerMask groundLayer;
    public Vector2 groundCapsuleCastSize = new Vector2(1.475f, 0.1f);
    public float maxSlopeAngle = 60f;
    public float lateralOffset = 0f;
    public float groundRayCastDistance = 0.3f;

    [Header("Rotation settings")]
    public float maxRotationSpeed = 1250f;
    public float minRotationSpeed = 500f;
    public float distanceRotationMin = 4f;
    public float distanceRotationMax = 1.25f;
    public float currentRotationSpeed = 0f;

    [Header("Movement settings")]
    public float moveForceMagnitude = 600f;
    public float currentMaxMoveSpeed = 9f;
    public float defaultMaxMoveSpeed = 9f;
    public float weakGravityMoveSpeedMult = 0.75f;
    public float strongGravityMoveSpeedMult = 0.5f;
    public float moveForceMultOnSlopes = 1.3f;

    [Header("Air movement settings")]
    public float moveAirForceMult = 1f;
    public float gravityBoostMult = 8f;

    [Header("Jump settings")]
    public float jumpForceMagnitude = 30000f;
    public float currentJumpBoostMult = 0.5f;
    public float defaultJumpBoostMult = 0.5f;
    public float weakGravityJumpBoostMult = 0.85f;
    public float jumpBoostStopVelocityThreshold = 1.5f;

    [Header("Crouch settings")]
    public float crouchMoveSpeedMult = 0.5f;
    public Vector2 crouchColliderSize = new Vector2(22.5f, 27f);
    public Vector2 crouchColliderOffset = new Vector2(5.4f, -6f);

    [Header("Obstacle detection settings")]
    public LayerMask obstaclesLayer;

    [Header("Control bools")]
    public bool isGrounded = true;
    public bool wasGrounded = true;
    public bool isFacingRight = true;
    public bool isRotating;
    public bool wasRotating;
    public bool isOnHighSlope;
    public bool wasOnHighSlope;
    public bool isBackOnWall;
    public bool wasBackOnWall;
    public bool canWalljump;
    public bool isGrabbing;
    public bool isDashing;
    public bool isCrouching;
    public bool isFacingObstacle;
    public bool playerDetected = false;

    private void Awake() {
        if (body == null)
            body = GetComponent<Rigidbody2D>();

        if (bodyCollider == null) {
            bodyCollider = GetComponent<CapsuleCollider2D>();
        }

        GetColliderSize();
    }

    public void ManualDestroy() {
        Destroy(gameObject);
    }


    private void Start() {
        gManager = GravityManager.instance;
        UpdateGravity();

        MainComponentsDeactivation(false);
    }

    //private void Update() {
    //    if (gManager.physicsGravity != currentGravity) {
    //        UpdateGravity();
    //    }
    //}

    public void UpdateGravity() {
        currentGravity = gManager.physicsGravity;
        currentGravityNormal = gManager.physicsGravityNormal;
        currentGravityRatio = gManager.gravityRatio;

        AdaptParametersToGravity();

        characterController.UpdateGravityForce();
    }

    private void AdaptParametersToGravity() {
        if (currentGravityRatio == 1)
            DefaultGravityParams();
        else {
            if (currentGravityRatio < 1)
                WeakGravityParams();
            else
                StrongGravityParams();
        }

    }

    private void DefaultGravityParams() {
        currentMaxMoveSpeed = defaultMaxMoveSpeed;
        currentJumpBoostMult = defaultJumpBoostMult;
    }

    private void WeakGravityParams() {
        currentMaxMoveSpeed = defaultMaxMoveSpeed * weakGravityMoveSpeedMult;
        currentJumpBoostMult = weakGravityJumpBoostMult;
    }

    private void StrongGravityParams() {
        currentMaxMoveSpeed = defaultMaxMoveSpeed * strongGravityMoveSpeedMult;
        currentJumpBoostMult = defaultJumpBoostMult;
    }

    private void GetColliderSize() {
        defaultColliderSize = bodyCollider.size;
        defaultColliderOffset = bodyCollider.offset;
        scaledColliderSize = bodyCollider.size * transform.localScale;
        scaledCrouchedColliderSize = crouchColliderSize * transform.localScale;
    }

    private void OnBecameVisible() {
        MainComponentsDeactivation(true);
    }

    private void OnBecameInvisible() {
        
        MainComponentsDeactivation(false);
    }

    private void MainComponentsDeactivation(bool state) {
        //print("setting main components to state: " + state);

        //bodyCollider.enabled = state;
        GetComponent<CharacterControllerAI>().enabled = state;
        GetComponent<MovementAI>().enabled = state;
        GetComponent<AnimationAI>().enabled = state;

        foreach (GameObject c in childrens) {
            c.SetActive(state);
        }

       
    }
}
