using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("References")]
    public static PlayerManager instance;
    public Rigidbody2D body;
    public CapsuleCollider2D bodyCollider;
    public Animator animator;
    public InputReader input;
    public CharacterControllerDynamic2D characterController;
    public PlayerMovement movement;
    public PlayerResources resources;
    public PlayerActions actions;

    [Header("Gravity info")]
    public Vector2 currentGravity;
    public Vector2 currentGravityNormal;
    public float currentGravityRatio;

    [Header("Rotation settings")]
    public float maxRotationSpeed;
    public float minRotationSpeed;
    public float distanceRotationMin;
    public float distanceRotationMax;
    [HideInInspector]
    public float rotationSpeed = 1f;

    [Header("Movement settings")]
    public float moveForceMagnitude = 10f;
    public float currentMaxMoveSpeed = 5f;
    public float defaultMaxMoveSpeed = 5f;
    public float weakGravityMoveSpeedMult = 0.75f;
    public float strongGravityMoveSpeedMult = 0.5f;

    [Header("Air movement settings")]
    public float moveAirForceMagnitude = 6f;
    public float gravityBoostMult = 0.5f;

    [Header("Jump settings")]
    public float jumpForceMagnitude = 200f;
    public float currentJumpBoostMult = 0.5f;
    public float defaultJumpBoostMult = 0.5f;
    public float weakGravityJumpBoostMult = 0.85f;
    public float jumpBoostVelocityThreshold = 1.5f;

    [Header("Walljump settings")]
    public Vector2 wallJumpDirection = new Vector2(0.5f, 0.5f);
    public float walljumpMult = 2f;

    [Header("Dash settings")]
    public float dashForceMult = 1f;
    public float dashTime = 0.2f;
    public float dashCheckDistance = 2f;
    public float dashCooldown = 1.5f;

    [Header("Control bools")]
    public bool isGrounded;
    public bool wasGrounded;
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

    [Header("Materials")]
    public PhysicsMaterial2D fullFrictionMaterial;
    public PhysicsMaterial2D defaultPhysicsMaterial;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        this.gameObject.transform.parent = null;
    }

    private void Start() {
        AdaptParametersToGravity();
    }

    private void Update() {
        GravityManager gManager = GravityManager.instance;

        if(gManager.physicsGravity != currentGravity) {
            currentGravity = gManager.physicsGravity;
            currentGravityNormal = gManager.physicsGravityNormal;
            currentGravityRatio = gManager.gravityRatio;

            AdaptParametersToGravity();
        }
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

}
