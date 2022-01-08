using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public enum GroundTypeEnum { 
    grass, 
    glass, 
    metal 
}

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
    public PlayerSounds sound;
    public PlayerEvents events;
    public PowerUpManager powersManager;

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
    //[HideInInspector]
    //public float rotationSpeed = 1f;

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

    [Header("Walljump settings")]
    public Vector2 wallJumpDirection = new Vector2(0.3f, 0.9f);
    public float walljumpForceMult = 2f;

    [Header("Dash settings")]
    public float dashForceMult = 0.5f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;

    [Header("Crouch settings")]
    public float crouchMoveSpeedMult = 0.5f;
    public Vector2 crouchColliderSize = new Vector2(22.5f, 27f);
    public Vector2 crouchColliderOffset = new Vector2(5.4f, -6f);

    [Header("Obstacle detection settings")]
    public LayerMask obstaclesLayer;
    //public float obstacleCheckDistanceMult = 0.6f;
    public Vector3 obstacleCheckOffset = new Vector3(0.125f, 0f, 0f);
    public Vector3 crouchedObstacleCheckOffset = new Vector3(1.15f, -1f, 0f);
    //public float obstacleCheckDistance;

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

    //[Header("Materials")]
    //public PhysicsMaterial2D fullFrictionMaterial;
    //public PhysicsMaterial2D defaultPhysicsMaterial;

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
        
        if (body == null)
            body = GetComponent<Rigidbody2D>();

        if (bodyCollider == null) {
            bodyCollider = GetComponent<CapsuleCollider2D>();
        }
    }

    public void ManualDestroy()
    {
        Destroy(gameObject);
    }

    private void Start() {
        AdaptParametersToGravity();
        GetColliderSize();
        currentMaxMoveSpeed = defaultMaxMoveSpeed;
    }

    private void OnEnable()
    {
        input.EnableInput();
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

    private void GetColliderSize() {
        defaultColliderSize = bodyCollider.size;
        defaultColliderOffset = bodyCollider.offset;
        scaledColliderSize = bodyCollider.size * transform.localScale;
        scaledCrouchedColliderSize = crouchColliderSize * transform.localScale;
    }

}
