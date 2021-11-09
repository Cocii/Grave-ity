using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("References")]
    public static PlayerManager instance;
    public Rigidbody2D body;
    public InputReader input;
    public Animator animator;
    public CharacterControllerDynamic2D characterControllerDynamic;

    [Header("Colliders")]
    public Collider2D crouchCollider;

    [Header("Info")]
    public Vector2 currentGravity;
    public Vector2 currentGravityNormal;

    [Header("Settings")]
    //public float artificialGravityScale = 1f;
    public bool usePhysics = true;
    public float rotationSpeed = 1f;
    public LayerMask groundLayer;

    [Header("Movement settings")]
    public float jumpForceMagnitude = 200f;
    public float jumoBoostMult = 1.75f;
    public float moveForceMagnitude = 10f;
    public float crouchSpeedMult = 0.5f;
    public float maxMoveSpeed;

    [Header("Control bools")]
    public bool isGrounded;
    public bool wasGrounded;
    public bool facingRight;

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
        if (usePhysics) {
            body.gravityScale = 1f;
            currentGravity = Physics2D.gravity;
            currentGravityNormal = GravityManager.instance.physicsGravityNormal;
        }
    }

    public void SetGravity(Vector2 gra, Vector2 graNorm) {
        usePhysics = false;
        body.gravityScale = 0f;
        currentGravity = gra;
        currentGravityNormal = graNorm;
    }

}
