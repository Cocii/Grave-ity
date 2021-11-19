using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("References")]
    public static PlayerManager instance;
    public Rigidbody2D body;
    public CapsuleCollider2D bodyCollider;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public InputReader input;
    public CharacterControllerDynamic2D characterController;
    public PlayerMovement movement;
    public PlayerResources resources;

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

    [Header("Air movement settings")]
    public float moveAirForceMagnitude = 6f;
    public float gravityBoostMult = 0.5f;

    [Header("Jump settings")]
    public float jumpForceMagnitude = 200f;
    public float jumpBoostMult = 1.75f;
    public Vector2 wallJumpDirection = new Vector2(0.5f, 0.5f);
    public float walljumpMult = 2f;

    [Header("Dash settings")]
    public float dashForceMult = 2f;

    [Header("Control bools")]
    public bool isGrounded;
    public bool wasGrounded;
    public bool isFacingRight = true;
    public bool isRotating;
    public bool wasRotating;
    public bool isOnHighSlope;
    public bool isBackOnWall;
    public bool wasBackOnWall;
    public bool canWalljump;

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
        currentMaxMoveSpeed = defaultMaxMoveSpeed * (2f - currentGravityRatio);
    }

}
