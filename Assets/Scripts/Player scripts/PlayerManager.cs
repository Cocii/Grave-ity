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

    [Header("Info")]
    public Vector2 currentGravity;
    public Vector2 currentGravityNormal;

    [Header("Rotation settings")]
    public float maxRotationSpeed;
    public float minRotationSpeed;
    public float distanceRotationMin;
    public float distanceRotationMax;
    [HideInInspector]
    public float rotationSpeed = 1f;

    [Header("Movement settings")]
    public float moveForceMagnitude = 10f;
    public float maxMoveSpeed;

    [Header("Jump settings")]
    public float jumpForceMagnitude = 200f;
    public float jumpBoostMult = 1.75f;

    [Header("Control bools")]
    public bool isGrounded;
    public bool wasGrounded;
    public bool facingRight;
    public bool isRotating;
    public bool wasRotating;
    
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
        currentGravity = Physics2D.gravity;
        currentGravityNormal = GravityManager.instance.physicsGravityNormal;
    }

    public void SetGravity(Vector2 gra, Vector2 graNorm) {
        //usePhysics = false;
        body.gravityScale = 0f;
        currentGravity = gra;
        currentGravityNormal = graNorm;
    }

}
