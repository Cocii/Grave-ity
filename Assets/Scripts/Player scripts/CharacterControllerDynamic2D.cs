using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerDynamic2D : MonoBehaviour
{
    [HideInInspector]
    public PlayerManager manager;

    [Header("Info")]
    public Vector2 gravityForce;
    public Vector2 moveForce;
    public Vector2 jumpForce;

    [Header("Ground check settings")]
    public Vector2 origin = new Vector2(0, -0.4f);
    public Vector2 size = new Vector2(0.4f, 0.04f);
    public float angle = 0;
    public float distance = 0;

    private void FixedUpdate() {
        if (manager == null)
            return;

        if (!manager.usePhysics)
            ApplyGravityForce();

        RotateTowardsGravity();

        ApplyMoveForce();
        ApplyJumpForce();
    }

    void Update() {
        manager = PlayerManager.instance;

        if (manager == null)
            return;

        gravityForce = manager.currentGravity;
        AdjustFlip();
        GroundCheck();
    }

    private void GroundCheck() {
        PlayerManager.instance.wasGrounded = PlayerManager.instance.isGrounded;
        PlayerManager.instance.isGrounded = false;

        //hit = Physics2D.Raycast(transform.position, -transform.up, 0.5f, PlayerManager.instance.groundLayer);

        Vector3 originAdjst =transform.position + new Vector3(origin.x, origin.y) * transform.up.y;

        //RaycastHit2D hit = Physics2D.BoxCast(originAdjst, size, angle, -transform.up, distance, PlayerManager.instance.groundLayer);
        //if (hit) {
        //    //print(hit.collider);
        //    PlayerManager.instance.isGrounded = true;
        //}

        //RaycastHit2D hitCapsule = Physics2D.CapsuleCast(originAdjst, size, CapsuleDirection2D.Horizontal, 0f, Vector2.right, 0, manager.groundLayer);
        //if (hitCapsule) {
        //    //print(hitCapsule.collider);
        //    PlayerManager.instance.isGrounded = true;
        //}

        Collider2D hitOverlap = Physics2D.OverlapCapsule(originAdjst, size, CapsuleDirection2D.Horizontal, angle, manager.groundLayer);
        if (hitOverlap != null) {
            //print(hitOverlap);
            PlayerManager.instance.isGrounded = true;
        }


    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Vector3 originAdjst = transform.position + new Vector3(origin.x, origin.y) * transform.up.y;
        Gizmos.DrawCube(originAdjst, size);
        Gizmos.color = Color.white;
    }

    private void PlayerLanded() {

    }

    
    private void FlipFacing() {
        manager.facingRight = !manager.facingRight;

        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1f;
        transform.localScale = currentScale;
    }

    private void AdjustFlip() {
        bool upside = gravityForce.normalized == Vector2.up ? true : false;
        float flipMult = 1;
        Vector3 currentScale = transform.localScale;

        if (!upside) {
            if ((manager.facingRight && currentScale.x < 0) || (!manager.facingRight && currentScale.x > 0)) {
                flipMult = -1f;
            }
        }
        else {
            if ((manager.facingRight && currentScale.x > 0) || (!manager.facingRight && currentScale.x < 0)) {
                flipMult = -1f;
            }
        }

        currentScale.x *= flipMult;
        transform.localScale = currentScale;
    }

    private void CheckFlip() {
        Vector2 velocity = manager.body.velocity;

        if (gravityForce.normalized == Vector2.down) {
            if ((velocity.x > 0.01f) && !manager.facingRight) {
                //Flip(true);
                FlipFacing();
            }
            else if ((velocity.x < -0.01f) && manager.facingRight) {
                //Flip(false);
                FlipFacing();
            }
        }
        else if (gravityForce.normalized == Vector2.up) {
            if ((velocity.x > 0.01f) && !manager.facingRight) {
                //Flip(true);
                FlipFacing();
            }
            else if ((velocity.x < -0.01f) && manager.facingRight) {
                //Flip(false);
                FlipFacing();
            }


        }

    }

    private void RotateTowardsGravity() {
        float targetAngle = 0f;
        bool facingRight = PlayerManager.instance.facingRight;

        if (PlayerManager.instance.usePhysics) {
            if (facingRight)
                targetAngle = GravityManager.instance.physicsGravityTransform.eulerAngles.z - 180f;
            else
                targetAngle = GravityManager.instance.physicsGravityTransform.eulerAngles.z + 180f;
        }
        
        float smoothedAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, PlayerManager.instance.rotationSpeed * Time.deltaTime);

        if (Mathf.Abs(smoothedAngle - Mathf.RoundToInt(smoothedAngle)) < 0.05f) {
            smoothedAngle = Mathf.RoundToInt(smoothedAngle);
        }

        transform.eulerAngles = new Vector3(0f, 0f, smoothedAngle);

    }

    private void ApplyGravityForce() {
        manager.body.AddForce(gravityForce);
    }

    private void ApplyMoveForce() {
        CheckFlip();
   
        PlayerManager.instance.body.AddForce(moveForce);
        manager.body.velocity = Vector2.ClampMagnitude(manager.body.velocity, manager.maxMoveSpeed);
        
        moveForce = Vector2.zero;
    }

    private void ApplyJumpForce() {
        PlayerManager.instance.body.AddForce(jumpForce);
        jumpForce = Vector2.zero;
    }

    public void SetMoveForce(Vector2 force) {
        moveForce = force;
    }

    public void SetJumpForce(Vector2 force) {
        jumpForce = force;
    }

    public void AddMoveForce(Vector2 force) {
        moveForce += force;
    }

    public void AddJumpForce(Vector2 force) {
        jumpForce += force;
    }
}
